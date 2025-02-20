﻿using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    [SessionTimeout]
    public class TerminoPagoController : Controller
    {
        #region Variables
        fa_TerminoPago_Bus bus_terminopago = new fa_TerminoPago_Bus();
        fa_TerminoPago_Distribucion_Bus bus_termino_dist = new fa_TerminoPago_Distribucion_Bus();
        fa_TerminoPago_Distribucion_list List_fa_TerminoPago_Distribucion = new fa_TerminoPago_Distribucion_list();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        fa_TerminoPago_List Lista_TerminoPago = new fa_TerminoPago_List();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "ClienteTipo", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            fa_TerminoPago_Info model = new fa_TerminoPago_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            var lst = bus_terminopago.get_list(true);
            Lista_TerminoPago.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_terminopago(bool Nuevo=false)
        {
            //List<fa_TerminoPago_Info> model = bus_terminopago.get_list(true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_TerminoPago.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_terminopago", model);
        }

        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "ClienteTipo", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_TerminoPago_Info model = new fa_TerminoPago_Info
            {
                Lst_fa_TerminoPago_Distribucion = new List<fa_TerminoPago_Distribucion_Info>(),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };
            List_fa_TerminoPago_Distribucion.set_list(model.Lst_fa_TerminoPago_Distribucion, model.IdTransaccionSession);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(fa_TerminoPago_Info model)
        {
            model.Lst_fa_TerminoPago_Distribucion = List_fa_TerminoPago_Distribucion.get_list(model.IdTransaccionSession);
            if (bus_terminopago.validar_existe_IdTerminoPago(model.IdTerminoPago))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                return View(model);
            }
            if (!bus_terminopago.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdTerminoPago = model.IdTerminoPago, Exito = true });
        }

        public ActionResult Consultar(string IdTerminoPago = "", bool Exito=false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            fa_TerminoPago_Info model = bus_terminopago.get_info(IdTerminoPago);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "ClienteTipo", "Index");
            if (model.estado == false)
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            model.Lst_fa_TerminoPago_Distribucion = bus_termino_dist.get_list(IdTerminoPago);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            List_fa_TerminoPago_Distribucion.set_list(model.Lst_fa_TerminoPago_Distribucion, model.IdTransaccionSession);
            return View(model);
        }

        public ActionResult Modificar(string IdTerminoPago = "")
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "ClienteTipo", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            fa_TerminoPago_Info model = bus_terminopago.get_info(IdTerminoPago);
            if (model == null)
                return RedirectToAction("Index");
            model.Lst_fa_TerminoPago_Distribucion = bus_termino_dist.get_list(IdTerminoPago);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            List_fa_TerminoPago_Distribucion.set_list(model.Lst_fa_TerminoPago_Distribucion, model.IdTransaccionSession);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(fa_TerminoPago_Info model)
        {
            model.Lst_fa_TerminoPago_Distribucion = List_fa_TerminoPago_Distribucion.get_list(model.IdTransaccionSession);

            if (!bus_terminopago.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdTerminoPago = model.IdTerminoPago, Exito = true });
        }

        public ActionResult Anular(string IdTerminoPago = "")
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "ClienteTipo", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            fa_TerminoPago_Info model = bus_terminopago.get_info(IdTerminoPago);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.Lst_fa_TerminoPago_Distribucion = bus_termino_dist.get_list(IdTerminoPago);
            List_fa_TerminoPago_Distribucion.set_list(model.Lst_fa_TerminoPago_Distribucion, model.IdTransaccionSession);
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(fa_TerminoPago_Info model)
        {
            if (!bus_terminopago.anularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region GRids

        [ValidateInput(false)]
        public ActionResult GridViewPartial_pago_dist()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = List_fa_TerminoPago_Distribucion.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_pago_dist", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] fa_TerminoPago_Distribucion_Info info_det)
        {
            if (ModelState.IsValid)
            List_fa_TerminoPago_Distribucion.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = List_fa_TerminoPago_Distribucion.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            
            return PartialView("_GridViewPartial_pago_dist", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] fa_TerminoPago_Distribucion_Info info_det)
        {
            if (ModelState.IsValid)
                List_fa_TerminoPago_Distribucion.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = List_fa_TerminoPago_Distribucion.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_pago_dist", model);
        }

        public ActionResult EditingDelete(int Secuencia)
        {
            List_fa_TerminoPago_Distribucion.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            fa_TerminoPago_Info model = new fa_TerminoPago_Info();
            model.Lst_fa_TerminoPago_Distribucion =  List_fa_TerminoPago_Distribucion.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_pago_dist", model);
        }
        public void CargarCuotas(int NumeroCuotas = 0, int DiasVcto = 0, decimal IdTransaccionSession = 0)
        {
            List<fa_TerminoPago_Distribucion_Info> lst_distribucion = new List<fa_TerminoPago_Distribucion_Info>();
            if (NumeroCuotas != 0 & DiasVcto != 0 && DiasVcto > NumeroCuotas)
            {
                int Dias = DiasVcto / NumeroCuotas;
                int DiasAcum = Dias;
                for (int i = 0; i < NumeroCuotas; i++)
                {
                    lst_distribucion.Add(new fa_TerminoPago_Distribucion_Info
                    {
                        Num_Dias_Vcto = DiasAcum,
                        Por_distribucion = (((float)DiasVcto / NumeroCuotas) / DiasVcto)*100
                    });
                    DiasAcum += Dias;
                }
            }
            List_fa_TerminoPago_Distribucion.set_list(lst_distribucion, IdTransaccionSession);
        }
        #endregion

    }
    public class fa_TerminoPago_List
    {
        string Variable = "fa_TerminoPago_Info";
        public List<fa_TerminoPago_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_TerminoPago_Info> list = new List<fa_TerminoPago_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_TerminoPago_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_TerminoPago_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class fa_TerminoPago_Distribucion_list
    {
        string variable = "fa_TerminoPago_Distribucion_Info";
        public List<fa_TerminoPago_Distribucion_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_TerminoPago_Distribucion_Info> list = new List<fa_TerminoPago_Distribucion_Info>();

                HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_TerminoPago_Distribucion_Info>)HttpContext.Current.Session[variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_TerminoPago_Distribucion_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
        }
        public void AddRow(fa_TerminoPago_Distribucion_Info info_det, decimal IdTransaccionSession)
        {
            List<fa_TerminoPago_Distribucion_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            info_det.IdTerminoPago = info_det.IdTerminoPago;
            info_det.Por_distribucion = info_det.Por_distribucion;
            info_det.Num_Dias_Vcto = info_det.Num_Dias_Vcto;

            list.Add(info_det);
        }

        public void UpdateRow(fa_TerminoPago_Distribucion_Info info_det, decimal IdTransaccionSession)
        {
            fa_TerminoPago_Distribucion_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            edited_info.IdTerminoPago = info_det.IdTerminoPago;
            edited_info.Por_distribucion = info_det.Por_distribucion;
            edited_info.Num_Dias_Vcto = info_det.Num_Dias_Vcto;

        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<fa_TerminoPago_Distribucion_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.Secuencia == Secuencia).First());
        }
    }
}