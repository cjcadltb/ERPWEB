﻿using Core.Erp.Data.General;
using Core.Erp.Info.General;
using System;
using System.Collections.Generic;

namespace Core.Erp.Bus.General
{
    public class tb_sis_Documento_Tipo_Talonario_Bus
    {
        tb_sis_Documento_Tipo_Talonario_Data odata = new tb_sis_Documento_Tipo_Talonario_Data();

        public List<tb_sis_Documento_Tipo_Talonario_Info> get_list(int IdEmpresa, int IdSucursal, string CodDocumentoTipo, bool mostrar_anulados)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdSucursal, CodDocumentoTipo, mostrar_anulados);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public tb_sis_Documento_Tipo_Talonario_Info get_info(int IdEmpresa, string CodDocumentoTipo, string Establecimiento, string PuntoEmision, string NumDocumento)
        {
            try
            {
                return odata.get_info(IdEmpresa, CodDocumentoTipo, Establecimiento, PuntoEmision, NumDocumento);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(tb_sis_Documento_Tipo_Talonario_Info info)
        {
            try
            {
                return odata.guardarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool modificarDB(tb_sis_Documento_Tipo_Talonario_Info info)
        {
            try
            {
                return odata.modificarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool anularDB(tb_sis_Documento_Tipo_Talonario_Info info)
        {
            try
            {
                return odata.anularDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string get_NumeroDocumentoInicial(int IdEmpresa, string CodDcumentoTipo, string Establecimiento, string PuntoEmision)
        {
            try
            {
                return odata.get_NumeroDocumentoInicial(IdEmpresa, CodDcumentoTipo, Establecimiento, PuntoEmision);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public tb_sis_Documento_Tipo_Talonario_Info GetUltimoNoUsado(int IdEmpresa, string CodDocumentoTipo, string Establecimiento, string PuntoEmision, bool EsDocumentoElectronico, bool Actualizar)
        {
            try
            {
                return odata.GetUltimoNoUsado(IdEmpresa, CodDocumentoTipo, Establecimiento, PuntoEmision, EsDocumentoElectronico, Actualizar);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<tb_sis_Documento_Tipo_Talonario_Info> get_list_actualizacion_masiva(int IdEmpresa, int IdSucursal, string CodDocumentoTipo, string Establecimiento, string PuntoEmision, int NumInicio, int NumFin)
        {
            try
            {
                return odata.get_list_actualizacion_masiva(IdEmpresa, IdSucursal, CodDocumentoTipo, Establecimiento, PuntoEmision, NumInicio, NumFin);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificacionMasivaDB(List<tb_sis_Documento_Tipo_Talonario_Info> info)
        {
            try
            {
                return odata.ModificacionMasivaDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GeneraClaveAcceso(DateTime fechaEmision, string tipoComprobante, string ruc, string serie, string numeroComprobante, string codigoNumerico = "12345678", string tipoEmision = "1", string ambiente = "2")
        {
            try
            {
                return odata.GeneraClaveAcceso(fechaEmision, tipoComprobante, ruc, serie, numeroComprobante, codigoNumerico, tipoEmision, ambiente);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
