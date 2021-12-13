using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidades;

namespace EjercicioIntegrador
{
    public partial class FormIntegrador : Form
    {
        List<Planeta> list = new List<Planeta>(80);
        Planeta planeta = new Planeta();
        public FormIntegrador()
        {
            InitializeComponent();
            this.richTextBox1.Clear();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

         /* 
           1- Crear un objeto de tipo Planeta.
           2- Serializarlo y mostrar en un MessageBox lo sucedido.
		   3- Si serializo, deserializarlo y mostrarlo en el RichTextBox.
         */
        private void btn1_Click(object sender, EventArgs e)
        {
            Planeta planeta = new Planeta(1, "Tierra", 1, 12.5);

            if(planeta.SerializarXml())
            {
                MessageBox.Show("Planeta Serializado");

               this.richTextBox1.Text = planeta.DeserializarXml();

            }
            else
            {
                MessageBox.Show("Error al intentar serializar");
            }
        }


        /*
        1- Crear tres objetos de tipo Planeta.
		2- Crear objeto de tipo SistemaSolar.
		3- Agregarlos planetas al sistema solar.
        4- Mostrarlos en el RichTextBox.
        */
        private void btn2_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Clear();

            // 1 -
            Planeta planeta1 = new Planeta(1, "Marte", 1, 12.5);
            Planeta planeta2 = new Planeta(1, "Jupiter", 2, 22.6);
            Planeta planeta3 = new Planeta(1, "Urano", 3, 32.7);

            // 2 - 
            SistemaSolar<Planeta> sistemaSolar = new SistemaSolar<Planeta>(3);

            // 3 -
            if(sistemaSolar.Agregar(planeta1) && sistemaSolar.Agregar(planeta2) 
                                              && sistemaSolar.Agregar(planeta3))
            {
                MessageBox.Show("planetas agregados");
            }
            else
            {
                MessageBox.Show("hubo un error");
            }

            // 4 -
            foreach(Planeta item in sistemaSolar.lista)
            {
                this.richTextBox1.AppendText(item.ToString());
                this.richTextBox1.AppendText("\n");
            }

        }



        /*
         1- Crear tres objetos de tipo Planeta.
		 2- Crear objeto de tipo SistemaSolar con capacidad=2.
		 3- Atrapar la Excepción "NoHayLugarException" en un bloque try-catch 
         4- Mostrar el mensaje de error en un MessageBox.
         */
        private void btn3_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Clear();

            // 1 -
            Planeta planeta1 = new Planeta(1, "Marte", 1, 12.5);
            Planeta planeta2 = new Planeta(1, "Jupiter", 2, 22.6);
            Planeta planeta3 = new Planeta(1, "Urano", 3, 32.7);

            // 2 - 
            SistemaSolar<Planeta> sistemaSolar = new SistemaSolar<Planeta>(2);

            // 3 - 4 
            try
            {
                sistemaSolar.Agregar(planeta1);
                sistemaSolar.Agregar(planeta2);
                sistemaSolar.Agregar(planeta3);
            }
            catch (NoHayLugarException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }


        /*
         * 1-Creo un objeto planeta
         * 2-Asociar evento al manejador "PlanetaConMuchaGravedad"
         * 3-Hago saltar el evento
         * 4-Atrapo y muestro la gravedad del planeta en el RichTextBox
         
         */
        private void btn4_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Clear();

            // 1 -
            Planeta planeta1 = new Planeta(1, "Marte", 1, 12.5);

            // 2 -
            planeta1.muchaGravedad += PlanetaConMuchaGravedad;

            // 3 - 
            planeta1.Gravedad = 31;

        }

        // 4 - 
        private void PlanetaConMuchaGravedad(double gravedad)
        {
            this.richTextBox1.AppendText("Se lanzo el evento y la gravedad es : ");
            this.richTextBox1.AppendText(gravedad.ToString());
        }


        /*
         1-Crea un Task, creo el método TraerPlanetas():void
         2-En el subproceso invoco al método TraerPlanetas()
         2-Traigo los planetas de la base de datos a través del hilo secundario.
         3-Modifico el ReachTextBox a través del hilo principal.
         */
        private void btnTraer_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Clear();

            Task hilo = Task.Run(() => this.TraerPlanetas());

        }

        private void TraerPlanetas()
        {

            if (this.richTextBox1.InvokeRequired)
            {
                // Genero un nuevo delegado y le asigno la funcion DoWork
                Action delegado = new Action(this.TraerPlanetas);

                this.list = AccesoDatos.ObtenerListaPlaneta();

                this.Invoke(delegado);
            }
            else
            {
                foreach(Planeta item in list)
                {
                    this.richTextBox1.AppendText(item.ToString());
                    this.richTextBox1.AppendText("---------------------------------\n");
                }
            }
        }


        /*
         1- Invoca al formulario de alta.
		 2- Si se acepta, se actualiza BD, se agrega a la lista.
         3- Retorna la lista y muestra en el RichTextBox.
         * */
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Limpio el richtextbox
            this.richTextBox1.Clear();

            FormAlta newAlta = new FormAlta(this.planeta);
            newAlta.StartPosition = FormStartPosition.CenterScreen;
            DialogResult ret = newAlta.ShowDialog();

            if(ret == DialogResult.OK)
            {
                if(AccesoDatos.AgregarPlaneta(this.planeta))
                {
                    MessageBox.Show("Se agrego el planeta!");

                    // Genero una lista de planetas para no repetir los datos en this.list
                    List<Planeta> listaShowAgregar = new List<Planeta>();

                    // Obtengo la lista de la Base de Datos
                    listaShowAgregar = AccesoDatos.ObtenerListaPlaneta();

                    // Los muestro
                    foreach (Planeta item in listaShowAgregar)
                    {
                        this.richTextBox1.AppendText(item.ToString());
                        this.richTextBox1.AppendText("---------------------------------\n");
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo agregar el planeta!");
                }
            }
            
        }

        /*
         1 Invoca el formulario de alta a modificar pasandole los datos para mostrar.
		 2- Si se acepta el modificado, se actualiza BD 
         3-retorna la lista y muestra en el RichTextBox.
         * */
        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Limpio el richtextbox
            this.richTextBox1.Clear();

            FormAlta newAlta = new FormAlta(this.planeta);
            newAlta.StartPosition = FormStartPosition.CenterScreen;
            DialogResult ret = newAlta.ShowDialog();

            if (ret == DialogResult.OK)
            {
                if (AccesoDatos.ModificarPlaneta(this.planeta))
                {
                    MessageBox.Show("Se modificaron los datos");

                    // Genero una lista de planetas para no repetir los datos en this.list
                    List<Planeta> listaShowAgregar = new List<Planeta>();

                    // Obtengo la lista de la Base de Datos
                    listaShowAgregar = AccesoDatos.ObtenerListaPlaneta();

                    // Los muestro
                    foreach (Planeta item in listaShowAgregar)
                    {
                        this.richTextBox1.AppendText(item.ToString());
                        this.richTextBox1.AppendText("---------------------------------\n");
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraro un planeta con esos datos!");
                }
            }
        }



        /*
         1- Invoca el formulario de alta a eliminar pasandole los datos para mostrar.
		 2- Si se acepta el eliminado, se actualiza BD
         3-retorna la lista y muestra.
         */
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Limpio el richtextbox
            this.richTextBox1.Clear();

            FormAlta newAlta = new FormAlta(this.planeta);
            newAlta.StartPosition = FormStartPosition.CenterScreen;
            DialogResult ret = newAlta.ShowDialog();
            int id = AccesoDatos.DevolverIdPlaneta(this.planeta);

            if (ret == DialogResult.OK && id != 0)
            {
                
                if (AccesoDatos.Eliminar(id))
                {
                    MessageBox.Show("Se elimino el planeta");

                    // Genero una lista de planetas para no repetir los datos en this.list
                    List<Planeta> listaShowAgregar = new List<Planeta>();

                    // Obtengo la lista de la Base de Datos
                    listaShowAgregar = AccesoDatos.ObtenerListaPlaneta();

                    // Los muestro
                    foreach (Planeta item in listaShowAgregar)
                    {
                        this.richTextBox1.AppendText(item.ToString());
                        this.richTextBox1.AppendText("---------------------------------\n");
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el planeta!");
                }
            }
            else
            {
                MessageBox.Show("No se encontraron planetas con esos datos!");
            }
        }


       
    }
}
