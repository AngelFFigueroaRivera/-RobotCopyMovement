using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
/* -- Bibliotecas añadidas --*/
using Microsoft.Kinect;
using System.IO;
using System.IO.Ports;
using System.Windows.Threading;
using System.Threading;
using System.Media;

/*---------------------------*/

namespace Ejemplo_PlantillaSkeleton
{
    /// <summary>
    /// Capítulo: Reflejar el movimiento con imágenes
    /// Ejemplo: Obtener la posición de la mano derecha (De cualquier persona, no se selecciona cual)
    /// Descripción: 
    ///              Este sencillo ejemplo muestra una ventana con un círculo del cual, su movimiento, refleja el 
    ///              movimiento de la mano derecha. Conforme se mueve la mano se mueve el círculo.
    /// </summary>
    public partial class MainWindow : Window
    {
        private KinectSensor miKinect;  //Representa el Kinect conectado

        /* ----------------------- Área para las variables ------------------------- */
        private WriteableBitmap imagen; //Se utiliza para generar la imagen a partir del arreglo de bytes recibidos
        private byte[] cantidadPixeles; //Arreglo para recibir los bytes que envía el Kinect
        /* ------------------------------------------------------------------------- */

        /* ----------------------- Área para las variables ------------------------- */
        double dMano_X_D;            //Representa la coordenada X de la mano derecha
        double dMano_Y_D;            //Representa la coordenada Y de la mano derecha
        double dMano_X_I;
        double dMano_Y_I;
        double dRodilla_X_D;
        double dRodilla_Y_D;
        double dRodilla_X_I;
        double dRodilla_Y_I;
        double dCabeza_X;
        double dCabeza_Y;
        int x1 = 0, x2 = 0, x3 = 0, x4 = 0, x5 = 0;

        SerialPort serialPort1 = new SerialPort("COM8", 115200);

        DispatcherTimer timer;

        DispatcherTimer timer2;


        Point joint_Point = new Point(); //Permite obtener los datos del Joint
        /* ------------------------------------------------------------------------- */

        public MainWindow()
        {
            InitializeComponent();
            // Realizar configuraciones e iniciar el Kinect
            Kinect_Config();

            try
            {
                serialPort1.Open();
                MessageBox.Show("INGRESASTE");
            }
            catch
            {
                MessageBox.Show("SIN CONEXION");
            }

            //Crea	el	DispatcherTimer
            timer = new DispatcherTimer();
            timer2 = new DispatcherTimer();
            //Especificar	el	intervalo	(cada	cuánto	tiempo	se	ejecuta	el	evento)
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            timer2.Interval = new TimeSpan(0, 0, 0, 1, 0 );
            //Crear	el	evento
            timer.Tick += new EventHandler(Timer_Tick);
            timer2.Tick += new EventHandler(Timer_Tick2);
            //Iniciar	el	evento
            timer.IsEnabled = true;
            timer2.IsEnabled = true;
        }


        private void usarSkeleton(Skeleton skeleton)
        {
            Joint joint1 = skeleton.Joints[JointType.HandRight];
            Joint joint2 = skeleton.Joints[JointType.HandLeft];
            Joint joint3 = skeleton.Joints[JointType.KneeRight];
            Joint joint4 = skeleton.Joints[JointType.KneeLeft];
            Joint joint5 = skeleton.Joints[JointType.Head];

            // Si el Joint está listo obtener las coordenadas
            if (joint1.TrackingState == JointTrackingState.Tracked)
            {

                // Obtener coordenadas
                joint_Point = this.SkeletonPointToScreen(joint1.Position);
                dMano_X_D = joint_Point.X;
                dMano_Y_D = joint_Point.Y;

                joint_Point = this.SkeletonPointToScreen(joint2.Position);
                dMano_X_I = joint_Point.X;
                dMano_Y_I = joint_Point.Y;

                joint_Point = this.SkeletonPointToScreen(joint3.Position);
                dRodilla_X_D = joint_Point.X;
                dRodilla_Y_D = joint_Point.Y;

                joint_Point = this.SkeletonPointToScreen(joint4.Position);
                dRodilla_X_I = joint_Point.X;
                dRodilla_Y_I = joint_Point.Y;

                joint_Point = this.SkeletonPointToScreen(joint5.Position);
                dCabeza_X = joint_Point.X;
                dCabeza_Y = joint_Point.Y;

                // Modificar coordenadas del indicador que refleja el movimiento (Ellipse rojo)
                Cabeza.SetValue(Canvas.TopProperty, dCabeza_Y - 12.5);
                Cabeza.SetValue(Canvas.LeftProperty, dCabeza_X - 12.5);

                ManoD.SetValue(Canvas.TopProperty, dMano_Y_D - 12.5);
                ManoD.SetValue(Canvas.LeftProperty, dMano_X_D - 12.5);

                ManoI.SetValue(Canvas.TopProperty, dMano_Y_I - 12.5);
                ManoI.SetValue(Canvas.LeftProperty, dMano_X_I - 12.5);

                RodillaD.SetValue(Canvas.TopProperty, dRodilla_Y_D - 12.5);
                RodillaD.SetValue(Canvas.LeftProperty, dRodilla_X_D - 12.5);

                RodillaI.SetValue(Canvas.TopProperty, dRodilla_Y_I - 12.5);
                RodillaI.SetValue(Canvas.LeftProperty, dRodilla_X_I - 12.5);

                // Indicar Id de la persona que es trazada
                LID.Content = skeleton.TrackingId;
            }
        }
        /* -- Área para el método que utiliza los datos proporcionados por Kinect -- */
        /// <summary>
        /// Método que realiza las manipulaciones necesarias sobre el Skeleton trazado
        /// </summary>
        /// 

        private void Timer_Tick2(object sender, EventArgs e)
        {
            string D = serialPort1.ReadLine();
            Di.Content = D;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Convert.ToInt16(dMano_Y_D) > x1 || Convert.ToInt16(dMano_Y_D) < x1)
            {
                x1 = Convert.ToInt16(dMano_Y_D);
            }
            
            if (Convert.ToInt16(dMano_Y_I) > x2 || Convert.ToInt16(dMano_Y_I) < x2)
            {
                x2 = Convert.ToInt16(dMano_Y_I);
            }
         
            if (Convert.ToInt16(dRodilla_Y_D) > x3 || Convert.ToInt16(dRodilla_Y_D) < x3)
            {
                x3 = Convert.ToInt16(dRodilla_Y_D);
            }

            if (Convert.ToInt16(dRodilla_Y_I) > x4 || Convert.ToInt16(dRodilla_Y_I) < x4)
            {
                x4 = Convert.ToInt16(dRodilla_Y_I);
            }

            if (Convert.ToInt16(dCabeza_X) > x5 || Convert.ToInt16(dCabeza_X) < x5)
            {
                x5 = Convert.ToInt16(dCabeza_X);
            }

            serialPort1.Write(Convert.ToString(x1 + " " + x2 + " " + x3 + " " + x4 + " " + x5 + "\n"));
            Console.WriteLine((Convert.ToString(x1 + " " + x2 + " " + x3 + " " + x4 + " " + x5)));
        }



        private void usarCamara()
        {
            // Escribir los datos en el Bitmap
            this.imagen.WritePixels(new Int32Rect(0, 0, this.imagen.PixelWidth, this.imagen.PixelHeight), this.cantidadPixeles, this.imagen.PixelWidth * sizeof(int), 0);
        }

       

        /// <summary>
        /// Metodo que convierte un "SkeletonPoint" a "DepthSpace", esto nos permite poder representar las coordenadas de los Joints
        /// en nuestra ventana en las dimensiones deseadas.
        /// </summary>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convertertir un punto a "Depth Space" en una resolución de 640x480
            DepthImagePoint depthPoint = this.miKinect.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }
        /* ------------------------------------------------------------------------- */

        /// <summary>
        /// Método que realiza las configuraciones necesarias en el Kinect 
        /// así también inicia el Kinect para el envío de datos
        /// </summary>
        private void Kinect_Config()
        {
            // Buscamos el Kinect conectado con la propiedad KinectSensors, al descubrir el primero con el estado Connected
            // se asigna a la variable miKinect que lo representará (KinectSensor miKinect)
            miKinect = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected);

            if (this.miKinect != null && !this.miKinect.IsRunning)
            {
                /* ------------------- Configuración del Kinect ------------------- */
                // Habilitar ColorStream con una resolución de 640x480 a una razón de 30 frames / seg
                this.miKinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                // Enlistar la función que se llamará cada vez que el Kinect tiene listo un frame de datos
                this.miKinect.ColorFrameReady += this.Kinect_FrameReady2;

                // Crear el arreglo que recibe los datos de los pixeles, FramePixelDataLength es el número de bytes en el frame
                this.cantidadPixeles = new byte[this.miKinect.ColorStream.FramePixelDataLength];

                // Crear el WriteableBitmap que tendrá la imagen
                this.imagen = new WriteableBitmap(this.miKinect.ColorStream.FrameWidth, this.miKinect.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                // Asignar la imagen como fuente para ser mostrada en la ventana
                this.Image.Source = this.imagen;
                /* ---------------------------------------------------------------- */

                /* ------------------- Configuración del Kinect ------------------- */
                // Habilitar el SkeletonStream para permitir el trazo de "Skeleton"
                this.miKinect.SkeletonStream.Enable();

                // Enlistar al evento que se ejecuta cada vez que el Kinect tiene datos listos
                this.miKinect.SkeletonFrameReady += this.Kinect_FrameReady;
                /* ---------------------------------------------------------------- */

                // Enlistar el método que se llama cada vez que hay un cambio en el estado del Kinect
                KinectSensor.KinectSensors.StatusChanged += Kinect_StatusChanged;

                // Iniciar el Kinect
                try
                {
                    this.miKinect.Start();
                }
                catch (IOException)
                {
                    this.miKinect = null;
                }
                LEstatus.Content = "Conectado";
            }
            else
            {
                // Enlistar el método que se llama cada vez que hay un cambio en el estado del Kinect
                KinectSensor.KinectSensors.StatusChanged += Kinect_StatusChanged;
            }
        }
        /// <summary>
        /// Método que adquiere los datos que envia el Kinect, su contenido varía según la tecnología 
        /// que se esté utilizando (Cámara, SkeletonTraking, DepthSensor, etc)
        /// </summary>
        /// 

        private void Kinect_FrameReady2(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    // Copiar los datos(referentes a los pixeles) del frame a un arreglo
                    colorFrame.CopyPixelDataTo(this.cantidadPixeles);

                    // Manipular los bytes en el arreglo
                    usarCamara();
                }
            }
        }

        private void Kinect_FrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            // Arreglo que recibe los datos  
            Skeleton[] skeletons = new Skeleton[0];
            Skeleton skeleton;

            // Abrir el frame recibido y copiarlo al arreglo skeletons
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            // Seleccionar el primer Skeleton trazado
            skeleton = (from trackSkeleton in skeletons where trackSkeleton.TrackingState == SkeletonTrackingState.Tracked select trackSkeleton).FirstOrDefault();

            if (skeleton == null)
            {
                LID.Content = "0";
                return;
            }
            LID.Content = skeleton.TrackingId;

            // Enviar el Skelton a usar
            this.usarSkeleton(skeleton);
        }
        /// <summary>
        /// Método que configura del Kinect de acuerdo a su estado(conectado, desconectado, etc),
        /// su contenido varia según la tecnología que se esté utilizando (Cámara, SkeletonTraking, DepthSensor, etc)
        /// </summary>
        private void Kinect_StatusChanged(object sender, StatusChangedEventArgs e)
        {

            switch (e.Status)
            {
                case KinectStatus.Connected:
                    if (this.miKinect == null)
                    {
                        this.miKinect = e.Sensor;
                    }

                    if (this.miKinect != null && !this.miKinect.IsRunning)
                    {
                        /* ------------------- Configuración del Kinect ------------------- */
                        //Habilitar ColorStream con una resolución de 640x480 a una razón de 30 frames/seg
                        this.miKinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                        //Enlistar la función que se llamará cada vez que el Kinect tenga listo un frame de datos
                        this.miKinect.ColorFrameReady += this.Kinect_FrameReady2;

                        // Crear el arreglo que recibe los datos de los pixeles, FramePixelDataLength es el número de bytes en el frame
                        this.cantidadPixeles = new byte[this.miKinect.ColorStream.FramePixelDataLength];

                        // Crear el WriteableBitmap que tendrá la imagen
                        this.imagen = new WriteableBitmap(this.miKinect.ColorStream.FrameWidth, this.miKinect.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                        // Asignar la imagen como fuente para ser mostrada en la ventana
                        this.Image.Source = this.imagen;
                        /* ---------------------------------------------------------------- */

                        /* ------------------- Configuración del Kinect ------------------- */
                        // Habilitar el SkeletonStream para permitir el trazo de "Skeleton"
                        this.miKinect.SkeletonStream.Enable();

                        // Enlistar al evento que se ejecuta cada vez que el Kinect tiene datos listos
                        this.miKinect.SkeletonFrameReady += this.Kinect_FrameReady;
                        /* ---------------------------------------------------------------- */

                        // Iniciar el Kinect
                        try
                        {
                            this.miKinect.Start();
                        }
                        catch (IOException)
                        {
                            this.miKinect = null;
                        }
                        LEstatus.Content = "Conectado";
                    }
                    break;
                case KinectStatus.Disconnected:
                    if (this.miKinect == e.Sensor)
                    {
                        /* ------------------- Configuración del Kinect ------------------- */
                        this.miKinect.ColorFrameReady -= this.Kinect_FrameReady2;
                        /* ---------------------------------------------------------------- */

                        /* ------------------- Configuración del Kinect ------------------- */
                        this.miKinect.SkeletonFrameReady -= this.Kinect_FrameReady;
                        /* ---------------------------------------------------------------- */

                        this.miKinect.Stop();
                        this.miKinect = null;
                        LEstatus.Content = "Desconectado";

                    }
                    break;
            }
        }
        /// <summary>
        /// Método que libera los recursos del Kinect cuando se termina la aplicación
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.miKinect != null && this.miKinect.IsRunning)
            {
                /* ------------------- Configuración del Kinect ------------------- */
                this.miKinect.ColorFrameReady -= this.Kinect_FrameReady2;
                /* ---------------------------------------------------------------- */

                /* ------------------- Configuración del Kinect ------------------- */
                this.miKinect.SkeletonFrameReady -= this.Kinect_FrameReady;
                /* ---------------------------------------------------------------- */

                this.miKinect.Stop();
            }
        }
    }
}
