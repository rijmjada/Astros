using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class SistemaSolar<T> where T : Planeta
    {
        #region Atributos

        public List<T> lista;
        protected int capacidad;

        #endregion

        #region Constructores
        public SistemaSolar()
        {
            this.lista = new List<T>();
        }

        public SistemaSolar(int capacidad) : this()
        {
            this.capacidad = capacidad;
        }

        #endregion

        #region Metodos

        public bool Agregar(Planeta planeta)
        {
            bool ret = false;

            if(this.capacidad > this.lista.Count)
            {
                this.lista.Add((T)planeta);
                ret = true;
            }
            else
            {
                throw new NoHayLugarException();
            }

            return ret;
        }

        #endregion

    }
}
/*
    En la misma biblioteca de clases:
    Crear clase SistemaSolar<T>:     (restringir para que sólo lo use Planeta)

    -Atributos:  (público) lista:List<T>
                 (protegido) capacidad:int

    -Constructores: 1-por defecto solo inicia la lista 
		    2-con parámetro que reciba la capacidad (reutilizar).

    -Métodos:  (público) Agregar(planeta:Planeta):bool -> Agrega el planeta a la lista y retorna true.
*/