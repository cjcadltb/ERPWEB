﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Banco
{
    public class ba_TipoFlujo_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public decimal IdTipoFlujo { get; set; }
        public Nullable<decimal> IdTipoFlujoPadre { get; set; }
        [Required(ErrorMessage = ("el campo descripción es obligatorio"))]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "el campo descripción debe tener mínimo 1 caracter y máximo 50")]
        public string Descricion { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "el campo código debe tener mínimo 1 caracter y máximo 50")]
        public string cod_flujo { get; set; }


        public bool EstadoBool { get; set; }
        public string IdUsuario { get; set; }
        public Nullable<System.DateTime> Fecha_Transac { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> Fecha_UltMod { get; set; }
        public string IdUsuarioUltAnu { get; set; }
        public Nullable<System.DateTime> Fecha_UltAnu { get; set; }
        public string MotiAnula { get; set; }

        public List<ba_Cbte_Ban_x_ba_TipoFlujo_Info> LstDet { get; set; }

        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }
    }
}
