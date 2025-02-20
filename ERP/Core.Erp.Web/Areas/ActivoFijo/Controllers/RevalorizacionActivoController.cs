﻿using Core.Erp.Bus.ActivoFijo;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.ActivoFijo;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Helps;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Areas.Contabilidad.Controllers;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.ActivoFijo.Controllers
{
    [SessionTimeout]

    public class RevalorizacionActivoController : Controller
    {
        #region Variables
        Af_Mej_Baj_Activo_Bus bus_activo = new Af_Mej_Baj_Activo_Bus();
        ct_cbtecble_det_List list_ct_cbtecble_det = new ct_cbtecble_det_List();
        ct_cbtecble_det_Bus bus_comprobante_detalle = new ct_cbtecble_det_Bus();
        Af_Activo_fijo_Bus bus_fijo = new Af_Activo_fijo_Bus();
        ct_cbtecble_tipo_Bus bus_tipo = new ct_cbtecble_tipo_Bus();
        ct_plancta_Bus bus_cuenta = new ct_plancta_Bus();
        string mensaje = string.Empty;
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        Af_Mej_Baj_Activo_List Lista_Revalorizacion = new Af_Mej_Baj_Activo_List();
        #endregion

        #region Index
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "RevalorizacionActivo", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            Af_Mej_Baj_Activo_Info model = new Af_Mej_Baj_Activo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_activo.get_list(model.IdEmpresa, true);
            Lista_Revalorizacion.set_list(lst, model.IdTransaccionSession);

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_rev_activo(bool Nuevo = false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Revalorizacion.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_rev_activo", model);
        }

        #endregion

        #region Metodos
        private bool validar(Af_Mej_Baj_Activo_Info i_validar, ref string msg)
        {
            if (i_validar.lst_ct_cbtecble_det.Count == 0)
            {
                mensaje = "Debe ingresar registros en el detalle, por favor verifique";
                return false;
            }
            if(i_validar.lst_ct_cbtecble_det.Sum(q=>q.dc_Valor)!=0)
            {
                mensaje = "La suma de los detalles debe ser 0, por favor verifique";
                return false;
            }
            foreach (var item in i_validar.lst_ct_cbtecble_det)
            {
                if (string.IsNullOrEmpty(item.IdCtaCble))
                {
                    mensaje = "Faltan cuentas contables, por favor verifique";
                    return false;
                }
            }
            if (i_validar.lst_ct_cbtecble_det.Where(q=>q.dc_Valor == 0).Count()>0)
            {
                mensaje = "Existen detalles con valor 0 en el debe o haber, por favor verifique";
                return false;
            }
            return true;
        }

        private void cargar_combos(int IdEmpresa)
        {
            Dictionary<string, string> lst_activo = new Dictionary<string, string>();
            lst_activo.Add("Mejo_Acti", "Mejora");
            lst_activo.Add("Baja_Acti", "Baja");
            ViewBag.lst_activo = lst_activo;

            var lst_fijo = bus_fijo.get_list(IdEmpresa, false);
            ViewBag.lst_fijo = lst_fijo;

            var lst_tipo = bus_tipo.get_list(IdEmpresa, false);
            ViewBag.lst_tipo = lst_tipo;

        }

        private void cargar_combos_detalle()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_cuentas = bus_cuenta.get_list(IdEmpresa, false, true);
            ViewBag.lst_cuentas = lst_cuentas;
        }
        #endregion

        #region Acciones

        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "RevalorizacionActivo", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            Af_Mej_Baj_Activo_Info model = new Af_Mej_Baj_Activo_Info
            {
                IdEmpresa = IdEmpresa,
                Fecha_MejBaj = DateTime.Now,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };

            model.lst_ct_cbtecble_det = new List<ct_cbtecble_det_Info>();
            list_ct_cbtecble_det.set_list(model.lst_ct_cbtecble_det,model.IdTransaccionSession);
            cargar_combos_detalle();
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(Af_Mej_Baj_Activo_Info model)
        {
            model.lst_ct_cbtecble_det = list_ct_cbtecble_det.get_list(model.IdTransaccionSession);
            if(!validar(model, ref mensaje))
            {
                cargar_combos(model.IdEmpresa);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdUsuario = SessionFixed.IdUsuario;
            if (!bus_activo.guardarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, Id_Mejora_Baja_Activo = model.Id_Mejora_Baja_Activo, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, decimal Id_Mejora_Baja_Activo = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            Af_Mej_Baj_Activo_Info model = bus_activo.get_info(IdEmpresa, Id_Mejora_Baja_Activo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "RevalorizacionActivo", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_ct_cbtecble_det = bus_comprobante_detalle.get_list(IdEmpresa, model.IdTipoCbte == null ? 0 : Convert.ToInt32(model.IdTipoCbte), model.IdCbteCble == null ? 0 : Convert.ToDecimal(model.IdCbteCble));
            list_ct_cbtecble_det.set_list(model.lst_ct_cbtecble_det, model.IdTransaccionSession);
            cargar_combos(IdEmpresa);
            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha_MejBaj, cl_enumeradores.eModulo.ACF, Convert.ToInt32(SessionFixed.IdSucursal), ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, decimal Id_Mejora_Baja_Activo = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "RevalorizacionActivo", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            Af_Mej_Baj_Activo_Info model = bus_activo.get_info(IdEmpresa, Id_Mejora_Baja_Activo);
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_ct_cbtecble_det = bus_comprobante_detalle.get_list(IdEmpresa, model.IdTipoCbte == null ? 0 : Convert.ToInt32(model.IdTipoCbte), model.IdCbteCble == null ? 0 : Convert.ToDecimal(model.IdCbteCble));
            list_ct_cbtecble_det.set_list(model.lst_ct_cbtecble_det,model.IdTransaccionSession);
            cargar_combos(IdEmpresa);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha_MejBaj, cl_enumeradores.eModulo.ACF, Convert.ToInt32(SessionFixed.IdSucursal), ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(Af_Mej_Baj_Activo_Info model)
        {
            model.lst_ct_cbtecble_det = list_ct_cbtecble_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                cargar_combos(model.IdEmpresa);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdUsuarioUltMod = Session["IdUsuario"].ToString();
            if (!bus_activo.modificarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, Id_Mejora_Baja_Activo = model.Id_Mejora_Baja_Activo, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0 , decimal Id_Mejora_Baja_Activo = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "RevalorizacionActivo", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            Af_Mej_Baj_Activo_Info model = bus_activo.get_info(IdEmpresa, Id_Mejora_Baja_Activo);
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.lst_ct_cbtecble_det = bus_comprobante_detalle.get_list(IdEmpresa, model.IdTipoCbte == null ? 0 : Convert.ToInt32(model.IdTipoCbte), model.IdCbteCble == null ? 0 : Convert.ToDecimal(model.IdCbteCble));
            list_ct_cbtecble_det.set_list(model.lst_ct_cbtecble_det,model.IdTransaccionSession);
            cargar_combos(IdEmpresa);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha_MejBaj, cl_enumeradores.eModulo.ACF, Convert.ToInt32(SessionFixed.IdSucursal), ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(Af_Mej_Baj_Activo_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_activo.anularDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Json

        public JsonResult get_valores(int IdActivoFijo = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            Af_Activo_fijo_Bus bus_activo = new Af_Activo_fijo_Bus();
            var resultado = bus_activo.get_valores(IdEmpresa, IdActivoFijo);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }

    public class Af_Mej_Baj_Activo_List
    {
        string Variable = "Af_Mej_Baj_Activo_Info";

        public List<Af_Mej_Baj_Activo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<Af_Mej_Baj_Activo_Info> list = new List<Af_Mej_Baj_Activo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<Af_Mej_Baj_Activo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<Af_Mej_Baj_Activo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}