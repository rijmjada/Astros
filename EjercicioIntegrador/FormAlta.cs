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
    public partial class FormAlta : Form
    {
        Planeta planeta;
        public FormAlta(Planeta planeta)
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            this.planeta = planeta;
        }
        
        //Recupera los datos de los txtBox y carga el atributo.
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(                    !String.IsNullOrWhiteSpace(this.txtNombre.Text) &&
                                   !String.IsNullOrWhiteSpace(this.txtSatelites.Text) &&
                                   !String.IsNullOrWhiteSpace(this.txtGravedad.Text)    ) 
            {
                this.planeta.nombre = this.txtNombre.Text;
                this.planeta.satelites = Convert.ToInt32(this.txtSatelites.Text);
                this.planeta.gravedad = Convert.ToDouble(this.txtGravedad.Text);

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Debe ingresar todos los campos!");
            }
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
