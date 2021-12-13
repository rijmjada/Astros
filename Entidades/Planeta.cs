using System;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;


/*
 * 
4) EVENTOS

-Crear Evento en Planeta "muchaGravedad"
-Crear DelegadoPlaneta(gravedad:double):void

-Creo la propiedad Gravedad sólo escritura: 
Asigna el atributo y dispara el evento si la gravedad supera 30 m/s2

 * */
namespace Entidades
{

    public delegate void DelegadoPlaneta(double gravedad);

    public class Planeta : ISerializable
    {

        public event DelegadoPlaneta muchaGravedad;

        #region Atributos
        public int id;
        public string nombre;
        public int satelites;
        public double gravedad;
        #endregion

        #region Propiedades

        public double Gravedad
        {
            set 
            { 
                if(value > 30)
                {
                    this.muchaGravedad(value);
                }

                this.gravedad = value;
            }
        }

        #endregion

        #region Constructores
        public Planeta()
        {
        }
        public Planeta(int id, string nombre, int satelites, double gravedad)
        {
            this.id = id;
            this.nombre = nombre;
            this.satelites = satelites;
            this.gravedad = gravedad;
        }
        #endregion

        #region Metodos
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.AppendLine("Id: " + this.id);
            ret.AppendLine("Nombre: " + this.nombre);
            ret.AppendLine("satelites: " + this.satelites);
            ret.AppendLine("gravedad: " + this.gravedad);

            return ret.ToString();
        } 
        #endregion

        #region ISerializable


        public string Path
        {
            get
            {
                string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string archivo = System.IO.Path.Combine(carpeta, "planetaSerializado.Xml");

                return archivo;
            }
        }

        public string DeserializarXml()
        {
            Planeta planeta = new Planeta();

            try
            {
                using (StreamReader sw = new StreamReader(this.Path))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Planeta));
                    planeta = xs.Deserialize(sw) as Planeta;
                }
            }
            catch (Exception)
            {
                //        Atrapar Expeciones          ! ! !
            }

            return planeta.ToString();
        }

        public bool SerializarXml()
        {
            bool ret = true;
            try
            {
                using (StreamWriter sw = new StreamWriter(this.Path))
                {
                    XmlSerializer xs = new XmlSerializer(this.GetType());
                    xs.Serialize(sw, this);
                }
            }
            catch (Exception e)
            {
                //        Atrapar Expeciones          ! ! !
            }

            return ret;
        }


        #endregion

    }
}
