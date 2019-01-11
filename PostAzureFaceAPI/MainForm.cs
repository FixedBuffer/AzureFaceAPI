using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.ProjectOxford.Face;
using System.IO;
using System.Reflection;

namespace PostAzureFaceAPI
{
    public partial class MainForm : Form
    {
        string FaceAPIKey = ConfigurationManager.AppSettings["FaceAPIKey"];
        string FaceAPIEndPoint = ConfigurationManager.AppSettings["FaceAPIEndPoint"];
        string GroupGUID = Guid.NewGuid().ToString();

        public MainForm()
        {
            InitializeComponent();
        }

        private async void btn_Train_Click(object sender, EventArgs e)
        {
            //Abrimos un dialogo de seleccion de carpetas
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //Si se ha seleccionado un directorio, hacemos su info
                DirectoryInfo directory = new DirectoryInfo(dialog.SelectedPath);

                //Comprobamos que el directorio tiene carpetas de personas
                if (directory.GetDirectories().Count() == 0)
                    return;

                //=====Empezamos a crear el grupo de trabajo
                //Creamos el cliente
                var faceServiceClient = new FaceServiceClient(FaceAPIKey, FaceAPIEndPoint);

                //Vamos a trabajar desde 0 siempre, asi que comprobamos si hay grupos, y si los hay los borramos
                var groups = await faceServiceClient.ListPersonGroupsAsync();
                foreach (var group in groups)
                {
                    await faceServiceClient.DeletePersonGroupAsync(group.PersonGroupId);
                }
                //Creamos un grupo
                await faceServiceClient.CreatePersonGroupAsync(GroupGUID, "FixedBuffer");

                foreach (var person in directory.GetDirectories())
                {
                    //Comprobamos que tenga imagenes
                    if (person.GetFiles().Count() == 0)
                        return;

                    //Obtenemos el nombre que le vamos a dar a la persona
                    var personName = person.Name;

                    lbl_Status.Text = $"Entrenando a {personName}";

                    //Añadimos a una persona al grupo
                    var personResult = await faceServiceClient.CreatePersonAsync(GroupGUID, personName);

                    //Añadimos todas las fotos a la persona
                    foreach (var file in person.GetFiles())
                    {
                        using (var fStream = File.OpenRead(file.FullName))
                        {
                            try
                            {
                                //Cargamos la imagen en el pictureBox
                                pct_Imagen.Image = new Bitmap(fStream);
                                //Reiniciamos el Stream
                                fStream.Seek(0, SeekOrigin.Begin);
                                // Actualizamos las caras en el servidor
                                var persistFace = await faceServiceClient.AddPersonFaceInPersonGroupAsync(GroupGUID, personResult.PersonId, fStream, file.FullName);
                            }
                            catch (FaceAPIException ex)
                            {
                                lbl_Status.Text = "";
                                MessageBox.Show($"Imposible seguir, razón:{ex.ErrorMessage}");
                                return;
                            }
                        }
                    }
                }

                try
                {
                    //Entrenamos el grupo con todas las personas que hemos metido
                    await faceServiceClient.TrainPersonGroupAsync(GroupGUID);

                    // Esperamos a que el entrenamiento acabe
                    while (true)
                    {
                        await Task.Delay(1000);
                        var status = await faceServiceClient.GetPersonGroupTrainingStatusAsync(GroupGUID);
                        if (status.Status != Microsoft.ProjectOxford.Face.Contract.Status.Running)
                        {
                            break;
                        }
                    }

                    //Si hemos llegado hasta aqui, el entrenamiento se ha completado
                    btn_Find.Enabled = true;
                    lbl_Status.Text = $"Entrenamiento completado";
                }
                catch (FaceAPIException ex)
                {
                    lbl_Status.Text = "";
                    MessageBox.Show($"Response: {ex.ErrorCode}. {ex.ErrorMessage}");
                }
                GC.Collect();
            }

        }

        private async void btn_Find_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".jpg";
            dialog.Filter = "Image files(*.jpg, *.png, *.bmp, *.gif) | *.jpg; *.png; *.bmp; *.gif";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var imagePath = dialog.FileName;

                //Creamos el cliente
                var faceServiceClient = new FaceServiceClient(FaceAPIKey, FaceAPIEndPoint);

                using (var fStream = File.OpenRead(imagePath))
                {
                    //Cargamos la imagen en el pictureBox
                    pct_Imagen.Image = new Bitmap(fStream);
                    //Reiniciamos el Stream
                    fStream.Seek(0, SeekOrigin.Begin);

                    try
                    {
                        //Detectamos las caras
                        var faces = await faceServiceClient.DetectAsync(fStream);

                        //Detectamos a las personas
                        var results = await faceServiceClient.IdentifyAsync(GroupGUID, faces.Select(ff => ff.FaceId).ToArray());

                        //Creamos una lista de caras y nombres asociados
                        List<(Guid, string)> detections = new List<(Guid, string)>();
                        foreach (var result in results)
                        {
                            //En caso de no haber encontrado un candidato, nos lo saltamos
                            if (result.Candidates.Length == 0)
                                continue;
                            var faceId = faces.FirstOrDefault(f => f.FaceId == result.FaceId).FaceId;
                            //Consultamos los datos de la persona detectada
                            var person = await faceServiceClient.GetPersonAsync(GroupGUID, result.Candidates[0].PersonId);

                            //Añadimos a la lista la relacion
                            detections.Add((faceId,person.Name));            
                        }

                        var faceBitmap = new Bitmap(pct_Imagen.Image);

                        using (var g = Graphics.FromImage(faceBitmap))
                        {                           

                            var br = new SolidBrush(Color.FromArgb(200, Color.LightGreen));

                            // Por cada cara reconocida
                            foreach (var face in faces)
                            {
                                var fr = face.FaceRectangle;
                                var fa = face.FaceAttributes;

                                var faceRect = new Rectangle(fr.Left, fr.Top, fr.Width, fr.Height);
                                Pen p = new Pen(br);
                                p.Width = 50;
                                g.DrawRectangle(p, faceRect);                                

                                // Calculamos la posicon del rectangulo
                                int rectTop = fr.Top + fr.Height + 10;
                                if (rectTop + 45 > faceBitmap.Height) rectTop = fr.Top - 30;

                                // Calculamos las dimensiones del rectangulo                     
                                g.FillRectangle(br, fr.Left - 10, rectTop, fr.Width < 120 ? 120 : fr.Width + 20, 125);

                                //Buscamos en la lista de relaciones cara persona
                                var person = detections.Where(x => x.Item1 == face.FaceId).FirstOrDefault();
                                var personName = person.Item2;
                                Font font = new Font(Font.FontFamily,90);
                                //Pintamos el nombre en la imagen
                                g.DrawString($"{personName}",
                                             font, Brushes.Black,
                                             fr.Left - 8,
                                             rectTop + 4);
                            }
                        }
                        pct_Imagen.Image = faceBitmap;

                    }
                    catch (FaceAPIException ex)
                    {

                    }
                }

            }
        }
    }
}
