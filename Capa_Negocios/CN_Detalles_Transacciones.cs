﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Objetos;
using Capa_Datos;

namespace Capa_Negocios
{
    public class CN_Detalles_Transacciones
    {
        private CD_Detalles_Transacciones CD_detTrans = new CD_Detalles_Transacciones();

        public List<Capa_Objetos.CO_Detalles_Transacciones> MostrarDetTrans(int id)
        {
            List<Capa_Objetos.CO_Detalles_Transacciones> tabla = new List<Capa_Objetos.CO_Detalles_Transacciones>();
            tabla = CD_detTrans.Read(id);
            return tabla;
        }
    }
}
