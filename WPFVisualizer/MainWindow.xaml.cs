using Cysterny;
using HelixToolkit.Wpf;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace WPFVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            double z = 0;
            foreach (var ksztalt in KsztaltyAttribute.DostepneKsztalty())
            {
                var tekst = new TextVisual3D();
                tekst.Text = ksztalt.Nazwa;
                tekst.Position = new Point3D(0, 0, z);
                z += tekst.Height;
                viewPort3d.Children.Add(tekst);
            }

        }

        StreamReader sr = null;

        private void viewPort3d_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var z = new Zadanie();
            if (sr != null && z.Wczytaj(sr))
            {
                try
                {
                    z.ObliczPoziomWody();
                }
                catch (Exception ex)
                {
                    Thread t = new Thread(delegate ()
                    {
                        MessageBox.Show(ex.Message);
                    });
                    t.Start();
                }
                var o = ObjectBuilder.DodajObiektyZadania(z);
                ZmienWidok(o);
                return;
            }
            if (sr != null)
                sr.Dispose();
            sr = new StreamReader("..\\..\\..\\input.txt");
            viewPort3d_MouseDoubleClick(sender, e);
        }

        private void ZmienWidok(ModelVisual3D model)
        {
            viewPort3d.Children.Clear();
            viewPort3d.Children.Add(new DefaultLights());
            viewPort3d.Children.Add(model);
            CameraHelper.ZoomExtents(viewPort3d.Camera, viewPort3d.Viewport, 500);
            viewPort3d.UpdateLayout();
        }
    }
}
