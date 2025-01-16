﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitacoraModels;
using NPOI.SS.Util;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BitacoraLogic
{
    public class ExportarExcelLogic
    {
        public int ExportReporteDistribucion(string path, List<ReporteDistribucion> listaReporte, string encabezado, string subencabezado)
        {
            var resp = 1;
            FileStream streamDocumentoExcel = null;
            try
            {                
                List<string> listaUsuarios = (from x in listaReporte select x.Usuario).Distinct().ToList();
                var Distribucion = (from l in listaReporte where l.IdProyecto > 0 select l).ToList();
                var ActividadesExt = (from l in listaReporte where l.IdProyecto == 0 select l).ToList();

                listaUsuarios = (from u in listaUsuarios orderby u select u).ToList();

                using (streamDocumentoExcel = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook documentoExcel = new XSSFWorkbook();
                    ISheet hoja = documentoExcel.CreateSheet("Reporte Distribucion " + DateTime.Today.ToString("dd - MM - yyyy"));
                    ICreationHelper cH = documentoExcel.GetCreationHelper();

                    // establecemos los parámetros básicos del documento excel
                    NPOI.HPSF.SummaryInformation si = NPOI.HPSF.PropertySetFactory.CreateSummaryInformation();
                    si.Author = "Bitacora";
                    si.CreateDateTime = DateTime.Now;
                    si.Title = "Reporte";
                    
                    // estilos de los textos
                    NPOI.SS.UserModel.IFont letraNegritaBlanco = documentoExcel.CreateFont();
                    letraNegritaBlanco.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    letraNegritaBlanco.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    letraNegritaBlanco.FontHeightInPoints = (short)12;

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont NegritaBlanca11 = documentoExcel.CreateFont();
                    NegritaBlanca11.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    NegritaBlanca11.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    NegritaBlanca11.FontHeightInPoints = (short)14;

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont letraNegrita = documentoExcel.CreateFont();
                    letraNegrita.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    letraNegrita.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    letraNegrita.FontHeightInPoints = (short)10;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle verde = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    verde.SetFont(letraNegrita);
                    verde.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    verde.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey40Percent.Index;
                    verde.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    verde.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    verde.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Grey50Percent.Index;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTituloAzulF = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTituloAzulF.SetFont(NegritaBlanca11);
                    celdaTituloAzulF.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    celdaTituloAzulF.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                    celdaTituloAzulF.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTituloAzul = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTituloAzul.SetFont(letraNegritaBlanco);
                    celdaTituloAzul.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    celdaTituloAzul.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
                    celdaTituloAzul.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle grisClaro = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    
                    grisClaro.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                    grisClaro.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    grisClaro.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    grisClaro.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;

                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTextoNormal = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTextoNormal.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                    hoja.CreateRow(0);
                    hoja.CreateRow(1);
                    for (int i = 0; i < (listaUsuarios.Count() + 6); i++) { hoja.GetRow(0).CreateCell(i).CellStyle = celdaTituloAzulF; hoja.GetRow(1).CreateCell(i).CellStyle = celdaTituloAzul; }
                    hoja.GetRow(0).GetCell(0).SetCellValue(encabezado);
                    hoja.GetRow(1).GetCell(0).SetCellValue(subencabezado);
                    var cra = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, listaUsuarios.Count() + 2);
                    var craa = new NPOI.SS.Util.CellRangeAddress(1, 1, 0, listaUsuarios.Count() + 2);
                    hoja.AddMergedRegion(cra);
                    hoja.AddMergedRegion(craa);

                    // creamos la fila para los titulos
                    hoja.CreateRow(2);

                    // se aplica el estilo a las celdas de titulo
                    for (int i = 0; i < (listaUsuarios.Count() + 6); i++) { hoja.GetRow(2).CreateCell(i).CellStyle = verde; }

                    // creamos los titulos de las columnas  
                    hoja.GetRow(2).GetCell(0).SetCellValue(" CLIENTE ");
                    hoja.GetRow(2).GetCell(1).SetCellValue(" NOMBRE DEL PROYECTO ");                    
                    hoja.GetRow(2).GetCell(2).SetCellValue(" UNIDAD DE NEGOCIO ");
                    hoja.GetRow(2).GetCell(3).SetCellValue(" AREA DE NEGOCIO ");
                    hoja.GetRow(2).GetCell(4).SetCellValue(" ACTIVIDAD EXTRAORDINARIA ");


                    for (int i = 0; i < listaUsuarios.Count(); i++)
                        hoja.GetRow(2).GetCell(5 + i).SetCellValue(" " + listaUsuarios[i] + " ");

                    hoja.GetRow(2).GetCell(5 + listaUsuarios.Count()).SetCellValue(" TOTAL ");
                    // insertamos los datos
                    int row = 3;

                    var listaProyectos = Distribucion.Select(x => new { x.Clinete, x.Proyecto, x.Unidad, x.Area }).Distinct().ToList();
                    double total = 0;
                    foreach (var col in listaProyectos)
                    {
                        total = 0;
                        var horasUsuarios = (from l in Distribucion where l.Clinete == col.Clinete && l.Proyecto == col.Proyecto select l).ToList();

                        hoja.CreateRow(row);
                        hoja.GetRow(row).CreateCell(0).SetCellValue(" " + col.Clinete + " ");
                        hoja.GetRow(row).CreateCell(1).SetCellValue(" " + col.Proyecto + " ");                        
                        hoja.GetRow(row).CreateCell(2).SetCellValue(" " + col.Unidad + " ");
                        hoja.GetRow(row).CreateCell(3).SetCellValue(" " + col.Area + " ");
                        hoja.GetRow(row).CreateCell(4).SetCellValue("");
                        var x = 0;
                        foreach (var u in horasUsuarios)
                        {
                            for (int i = x; i < listaUsuarios.Count(); i++)
                            {
                                if (u.Usuario == listaUsuarios[i])
                                {
                                    
                                    hoja.GetRow(row).CreateCell(5 + i).SetCellValue((double)u.Horas);
                                    total = total + Convert.ToDouble(u.Horas);
                                    x = i + 1;
                                    break;
                                }
                                else
                                    hoja.GetRow(row).CreateCell(5 + i).SetCellValue("");
                            }
                        }
                        
                        hoja.GetRow(row).CreateCell(5 + listaUsuarios.Count()).SetCellValue((double)total);
                        row++;
                    }
                    
                    var listaActividades = ActividadesExt.Select(x => new {x.Unidad, x.Actividad }).Distinct().ToList();

                    foreach (var col in listaActividades)
                    {
                        total = 0;
                        var horasUsuarios = (from l in ActividadesExt where l.Unidad == col.Unidad && l.Actividad == col.Actividad select l).ToList();
                        horasUsuarios = (from h in horasUsuarios orderby h.Usuario select h).ToList();
                        hoja.CreateRow(row);
                        hoja.GetRow(row).CreateCell(0).SetCellValue("");
                        hoja.GetRow(row).CreateCell(1).SetCellValue("");                        
                        hoja.GetRow(row).CreateCell(2).SetCellValue(" " + col.Unidad +"  ");
                        hoja.GetRow(row).CreateCell(3).SetCellValue("");
                        hoja.GetRow(row).CreateCell(4).SetCellValue(" " + col.Actividad + " ");
                        var x = 0;
                        foreach (var u in horasUsuarios)
                        {
                            for (int i = x; i < listaUsuarios.Count(); i++)
                            {
                                if (u.Usuario == listaUsuarios[i])
                                {
                                    
                                    hoja.GetRow(row).CreateCell(5 + i).SetCellValue((double)u.Horas);
                                    x = i + 1;
                                    total = total + Convert.ToDouble(u.Horas);
                                    break;
                                }
                                else
                                    hoja.GetRow(row).CreateCell(5 + i).SetCellValue("");
                            }
                        }
                        hoja.GetRow(row).CreateCell(5 + listaUsuarios.Count()).SetCellValue((double)total);
                        row++;
                    }
                    hoja.CreateRow(row);
                    for (int i = 0; i < listaUsuarios.Count(); i++)
                    {
                        var t = (from x in listaReporte where x.Usuario == listaUsuarios[i] && (x.IdProyecto != 0 || x.IdActividad == 40 || x.IdActividad == 42 || x.IdActividad == 54) select x.Horas).Sum();
                        hoja.GetRow(row).CreateCell(4).CellStyle = grisClaro;
                        hoja.GetRow(row).GetCell(4).SetCellValue("Total operaciones");
                        hoja.GetRow(row).CreateCell(5 + i).CellStyle = grisClaro;
                        hoja.GetRow(row).GetCell(5 + i).SetCellValue(Convert.ToDouble(t));
                    }
                    double totalOperaciones = Convert.ToDouble((from x in listaReporte where (x.IdProyecto != 0 || x.IdActividad == 40 || x.IdActividad == 42 || x.IdActividad == 54) select x.Horas).Sum());
                    hoja.GetRow(row).CreateCell(5 + listaUsuarios.Count()).SetCellValue(Convert.ToDouble(totalOperaciones));

                    row++;
                    hoja.CreateRow(row);
                    for (int i = 0; i < listaUsuarios.Count(); i++)
                    {
                        var t = (from x in listaReporte where x.Usuario == listaUsuarios[i] && (x.IdProyecto == 0 &&  x.IdActividad != 40 && x.IdActividad != 42 && x.IdActividad != 54 && x.IdActividad != 41 && x.IdActividad != 45) select x.Horas).Sum();
                        hoja.GetRow(row).CreateCell(4).CellStyle = grisClaro; 
                        hoja.GetRow(row).GetCell(4).SetCellValue("Tiempo muerto");
                        hoja.GetRow(row).CreateCell(5 + i).CellStyle = grisClaro;
                        hoja.GetRow(row).GetCell(5 + i).SetCellValue(Convert.ToDouble(t));
                    }
                    double totalTiemposMuertos = Convert.ToDouble((from x in listaReporte where (x.IdProyecto == 0 && x.IdActividad != 40 && x.IdActividad != 42 && x.IdActividad != 54 && x.IdActividad != 41 && x.IdActividad != 45) select x.Horas).Sum());
                    
                    hoja.GetRow(row).CreateCell(5 + listaUsuarios.Count()).SetCellValue(Convert.ToDouble(totalTiemposMuertos));

                    row++;
                    hoja.CreateRow(row);
                    for (int i = 0; i < listaUsuarios.Count(); i++)
                    {
                        var t = (from x in listaReporte where x.Usuario == listaUsuarios[i] select x.Horas).Sum();
                        hoja.GetRow(row).CreateCell(4).CellStyle = grisClaro;
                        hoja.GetRow(row).GetCell(4).SetCellValue("Total");
                        hoja.GetRow(row).CreateCell(5 + i).CellStyle = grisClaro;
                        hoja.GetRow(row).GetCell(5 + i).SetCellValue(Convert.ToDouble(t));
                    }
                    
                    total = Convert.ToDouble((from x in listaReporte select x.Horas).Sum());  
                    hoja.GetRow(row).CreateCell(5 + listaUsuarios.Count()).SetCellValue(Convert.ToDouble(total));
                    // se establecen la longitud de las columnas             
                    for (int i = 0; i < (listaUsuarios.Count() + 5); i++){hoja.AutoSizeColumn(i);}

                    documentoExcel.Write(streamDocumentoExcel);
                    streamDocumentoExcel.Close();
                }       
            }
            catch (Exception ex)
            {
                if (streamDocumentoExcel != null) { streamDocumentoExcel.Close(); }

                throw ex;
                resp = -1;
            }
            return resp;
        }

        public int ExportReporteDetallado(string path, List<ReporteDetallado> listaReporte, string encabezado, string subencabezado)
        {
            var resp = 1;
            FileStream streamDocumentoExcel = null;
            try
            {
                using (streamDocumentoExcel = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook documentoExcel = new XSSFWorkbook();
                    ISheet hoja = documentoExcel.CreateSheet("Reporte Detallado " + DateTime.Today.ToString("dd - MM - yyyy"));
                    ICreationHelper cH = documentoExcel.GetCreationHelper();

                    // establecemos los parámetros básicos del documento excel
                    NPOI.HPSF.SummaryInformation si = NPOI.HPSF.PropertySetFactory.CreateSummaryInformation();
                    si.Author = "Bitacora";
                    si.CreateDateTime = DateTime.Now;
                    si.Title = "Reporte";

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont letraNegritaBlanco = documentoExcel.CreateFont();
                    letraNegritaBlanco.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    letraNegritaBlanco.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    letraNegritaBlanco.FontHeightInPoints = (short)12;

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont NegritaBlanca11 = documentoExcel.CreateFont();
                    NegritaBlanca11.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    NegritaBlanca11.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    NegritaBlanca11.FontHeightInPoints = (short)14;

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont letraNegrita = documentoExcel.CreateFont();
                    letraNegrita.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    letraNegrita.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    letraNegrita.FontHeightInPoints = (short)10;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle verde = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    verde.SetFont(letraNegrita);
                    verde.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    verde.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey40Percent.Index;
                    verde.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    verde.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    verde.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Grey50Percent.Index;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTituloAzulF = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTituloAzulF.SetFont(NegritaBlanca11);
                    celdaTituloAzulF.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    celdaTituloAzulF.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                    celdaTituloAzulF.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTituloAzul = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTituloAzul.SetFont(letraNegritaBlanco);
                    celdaTituloAzul.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    celdaTituloAzul.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
                    celdaTituloAzul.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTextoNormal = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTextoNormal.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    
                    hoja.CreateRow(0);
                    hoja.CreateRow(1);
                    for (int i = 0; i < 9; i++) { hoja.GetRow(0).CreateCell(i).CellStyle = celdaTituloAzulF; hoja.GetRow(1).CreateCell(i).CellStyle = celdaTituloAzul; }
                    hoja.GetRow(0).GetCell(0).SetCellValue(encabezado);
                    hoja.GetRow(1).GetCell(0).SetCellValue(subencabezado);
                    var cra = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 8);
                    var craa = new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 8);
                    hoja.AddMergedRegion(cra);
                    hoja.AddMergedRegion(craa);

                    // creamos la fila para los titulos
                    hoja.CreateRow(2);

                    // se aplica el estilo a las celdas de titulo
                    for (int i = 0; i < 10; i++) { hoja.GetRow(2).CreateCell(i).CellStyle = verde; }

                    // creamos los titulos de las columnas  
                    hoja.GetRow(2).GetCell(0).SetCellValue(" FECHA ");
                    hoja.GetRow(2).GetCell(1).SetCellValue(" HORAS ");
                    hoja.GetRow(2).GetCell(2).SetCellValue(" RECURSO ");
                    hoja.GetRow(2).GetCell(3).SetCellValue(" PROYECTO ");
                    hoja.GetRow(2).GetCell(4).SetCellValue("    UNIDAD DE NEGOCIO USUARIO    ");
                    hoja.GetRow(2).GetCell(5).SetCellValue("    UNIDAD DE NEGOCIO PROYECTO    ");
                    hoja.GetRow(2).GetCell(6).SetCellValue(" AREA DE NEGOCIO ");
                    hoja.GetRow(2).GetCell(7).SetCellValue(" ACTIVIDAD EXTRAORDINARIA ");
                    hoja.GetRow(2).GetCell(8).SetCellValue(" DETALLE ");
                    hoja.GetRow(2).GetCell(9).SetCellValue(" ETAPA ");

                                       
                    // insertamos los datos
                    int row = 3;


                    foreach (var col in listaReporte)
                    {
                        hoja.CreateRow(row);
                        hoja.GetRow(row).CreateCell(0).SetCellValue(" " + col.Fecha.ToString("dd/MM/yyyy") + "  "); 
                        hoja.GetRow(row).CreateCell(1).SetCellValue((double)col.Horas);
                        hoja.GetRow(row).CreateCell(2).SetCellValue(" " + col.Usuario + "  ");
                        hoja.GetRow(row).CreateCell(3).SetCellValue(" " + col.Proyecto + " ");
                        hoja.GetRow(row).CreateCell(4).SetCellValue(" " + col.UnidadUsuario + " ");
                        hoja.GetRow(row).CreateCell(5).SetCellValue(" " + col.Unidad + " ");
                        hoja.GetRow(row).CreateCell(6).SetCellValue(" " + col.Area + " ");
                        hoja.GetRow(row).CreateCell(7).SetCellValue(" " + col.Actividad + " ");
                        hoja.GetRow(row).CreateCell(8).SetCellValue(" " + col.Detalle + " ");
                        hoja.GetRow(row).CreateCell(9).SetCellValue(" " + col.Etapa + " ");                       

                        row++;
                    }

                    // se establecen la longitud de las columnas             
                    for (int i = 0; i < 10; i++) { hoja.AutoSizeColumn(i); }

                    documentoExcel.Write(streamDocumentoExcel);
                    streamDocumentoExcel.Close();
                }
            }
            catch (Exception ex)
            {
                if (streamDocumentoExcel != null) { streamDocumentoExcel.Close(); }

                throw ex;
                resp = -1;
            }
            return resp;
        }

        public int ExportReportePersonas(string path, List<ReportePersonas> listaReporte, string encabezado, string subencabezado)
        {
            var resp = 1;
            FileStream streamDocumentoExcel = null;
            try
            {
                using (streamDocumentoExcel = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook documentoExcel = new XSSFWorkbook();
                    ISheet hoja = documentoExcel.CreateSheet("Reporte Personas " + DateTime.Today.ToString("dd - MM - yyyy"));
                    ICreationHelper cH = documentoExcel.GetCreationHelper();

                    // establecemos los parámetros básicos del documento excel
                    NPOI.HPSF.SummaryInformation si = NPOI.HPSF.PropertySetFactory.CreateSummaryInformation();
                    si.Author = "Bitacora";
                    si.CreateDateTime = DateTime.Now;
                    si.Title = "Reporte";

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont letraNegritaBlanco = documentoExcel.CreateFont();
                    letraNegritaBlanco.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    letraNegritaBlanco.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    letraNegritaBlanco.FontHeightInPoints = (short)12;

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont NegritaBlanca11 = documentoExcel.CreateFont();
                    NegritaBlanca11.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    NegritaBlanca11.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    NegritaBlanca11.FontHeightInPoints = (short)14;

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont letraNegrita = documentoExcel.CreateFont();
                    letraNegrita.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    letraNegrita.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    letraNegrita.FontHeightInPoints = (short)10;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle verde = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    verde.SetFont(letraNegrita);
                    verde.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    verde.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey40Percent.Index;
                    verde.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    verde.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    verde.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Grey50Percent.Index;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTituloAzulF = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTituloAzulF.SetFont(NegritaBlanca11);
                    celdaTituloAzulF.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    celdaTituloAzulF.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                    celdaTituloAzulF.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTituloAzul = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTituloAzul.SetFont(letraNegritaBlanco);
                    celdaTituloAzul.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    celdaTituloAzul.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
                    celdaTituloAzul.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTextoNormal = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTextoNormal.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                    hoja.CreateRow(0);
                    hoja.CreateRow(1);
                    for (int i = 0; i < 3; i++) { hoja.GetRow(0).CreateCell(i).CellStyle = celdaTituloAzulF; hoja.GetRow(1).CreateCell(i).CellStyle = celdaTituloAzul; }
                    hoja.GetRow(0).GetCell(0).SetCellValue(encabezado);
                    hoja.GetRow(1).GetCell(0).SetCellValue(subencabezado);
                    var cra = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 2);
                    var craa = new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 2);
                    hoja.AddMergedRegion(cra);
                    hoja.AddMergedRegion(craa);
                    

                    // creamos la fila para los titulos
                    hoja.CreateRow(2);

                    // se aplica el estilo a las celdas de titulo
                    for (int i = 0; i < 3; i++) { hoja.GetRow(2).CreateCell(i).CellStyle = verde; }

                    // creamos los titulos de las columnas  
                    hoja.GetRow(2).GetCell(0).SetCellValue(" NOMBRE ");
                    hoja.GetRow(2).GetCell(1).SetCellValue(" HORAS ");
                    hoja.GetRow(2).GetCell(2).SetCellValue(" UNIDAD DE NEGOCIO");

                    // insertamos los datos
                    int row = 3;


                    foreach (var col in listaReporte)
                    {
                        hoja.CreateRow(row);
                        hoja.GetRow(row).CreateCell(0).SetCellValue(" " + col.Usuario + "  ");
                        hoja.GetRow(row).CreateCell(1).SetCellValue((double)col.Horas);
                        hoja.GetRow(row).CreateCell(2).SetCellValue(" " + col.Unidad + "  ");

                        row++;
                    }

                    // se establecen la longitud de las columnas             
                    hoja.SetColumnWidth(0, 5000);
                    hoja.SetColumnWidth(1, 2000);
                    hoja.SetColumnWidth(2, 9000);

                    documentoExcel.Write(streamDocumentoExcel);
                    streamDocumentoExcel.Close();
                }
            }
            catch (Exception ex)
            {
                if (streamDocumentoExcel != null) { streamDocumentoExcel.Close(); }

                throw ex;
                resp = -1;
            }
            return resp;
        }

        public int ExportReporteProyectos(string path, List<ReporteProyectos> listaReporte, string encabezado, string subencabezado)
        {
            var resp = 1;
            FileStream streamDocumentoExcel = null;
            try
            {
                var Proyectos = (from l in listaReporte where l.IdProyecto > 0 select l).ToList();
                var ActividadesExt = (from l in listaReporte where l.IdProyecto == 0 select l).ToList();

                using (streamDocumentoExcel = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook documentoExcel = new XSSFWorkbook();
                    ISheet hoja = documentoExcel.CreateSheet("Reporte Proyectos " + DateTime.Today.ToString("dd - MM - yyyy"));
                    ICreationHelper cH = documentoExcel.GetCreationHelper();

                    // establecemos los parámetros básicos del documento excel
                    NPOI.HPSF.SummaryInformation si = NPOI.HPSF.PropertySetFactory.CreateSummaryInformation();
                    si.Author = "Bitacora";
                    si.CreateDateTime = DateTime.Now;
                    si.Title = "Reporte";

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont letraNegritaBlanco = documentoExcel.CreateFont();
                    letraNegritaBlanco.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    letraNegritaBlanco.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    letraNegritaBlanco.FontHeightInPoints = (short)12;

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont NegritaBlanca11 = documentoExcel.CreateFont();
                    NegritaBlanca11.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    NegritaBlanca11.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    NegritaBlanca11.FontHeightInPoints = (short)14;

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont letraNegrita = documentoExcel.CreateFont();
                    letraNegrita.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    letraNegrita.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    letraNegrita.FontHeightInPoints = (short)10;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle verde = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    verde.SetFont(letraNegrita);
                    verde.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    verde.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey40Percent.Index;
                    verde.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    verde.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    verde.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Grey50Percent.Index;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTituloAzulF = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTituloAzulF.SetFont(NegritaBlanca11);
                    celdaTituloAzulF.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    celdaTituloAzulF.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                    celdaTituloAzulF.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTituloAzul = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTituloAzul.SetFont(letraNegritaBlanco);
                    celdaTituloAzul.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    celdaTituloAzul.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
                    celdaTituloAzul.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTextoNormal = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTextoNormal.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                    hoja.CreateRow(0);
                    hoja.CreateRow(1);
                    for (int i = 0; i < 6; i++) { hoja.GetRow(0).CreateCell(i).CellStyle = celdaTituloAzulF; hoja.GetRow(1).CreateCell(i).CellStyle = celdaTituloAzul; }
                    hoja.GetRow(0).GetCell(0).SetCellValue(encabezado);
                    hoja.GetRow(1).GetCell(0).SetCellValue(subencabezado);
                    var cra = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 6);
                    var craa = new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 6);
                    hoja.AddMergedRegion(cra);
                    hoja.AddMergedRegion(craa);

                    // creamos la fila para los titulos
                    hoja.CreateRow(2);

                    // se aplica el estilo a las celdas de titulo
                    for (int i = 0; i < 7; i++) { hoja.GetRow(2).CreateCell(i).CellStyle = verde; }

                    // creamos los titulos de las columnas  
                    hoja.GetRow(2).GetCell(0).SetCellValue(" CLIENTE ");
                    hoja.GetRow(2).GetCell(1).SetCellValue(" NOMBRE DEL PROYECTO ");
                    hoja.GetRow(2).GetCell(2).SetCellValue(" UNIDAD DE NEGOCIO ");
                    hoja.GetRow(2).GetCell(3).SetCellValue(" AREA DE NEGOCIO ");
                    hoja.GetRow(2).GetCell(4).SetCellValue(" ACTIVIDAD EXTRAORDINARIA ");
                    hoja.GetRow(2).GetCell(5).SetCellValue(" UNIDAD DE REGISTRO");
                    hoja.GetRow(2).GetCell(6).SetCellValue(" HORAS ");

                    // insertamos los datos
                    int row = 3;

                    foreach (var col in Proyectos)
                    {
                        hoja.CreateRow(row);
                        hoja.GetRow(row).CreateCell(0).SetCellValue(" " + col.Cliente + " ");
                        hoja.GetRow(row).CreateCell(1).SetCellValue(" " + col.Proyecto + " ");
                        hoja.GetRow(row).CreateCell(2).SetCellValue(" " + col.Unidad + " ");
                        hoja.GetRow(row).CreateCell(3).SetCellValue(" " + col.Area + " ");
                        hoja.GetRow(row).CreateCell(4).SetCellValue("");
                        hoja.GetRow(row).CreateCell(5).SetCellValue(" " + col.UnidadRegistro + " ");
                        hoja.GetRow(row).CreateCell(6).SetCellValue((double)col.Horas);
                        row++;
                    }

                    foreach (var col in ActividadesExt)
                    {
                        hoja.CreateRow(row);
                        hoja.GetRow(row).CreateCell(0).SetCellValue("");
                        hoja.GetRow(row).CreateCell(1).SetCellValue("");
                        hoja.GetRow(row).CreateCell(2).SetCellValue(" " + col.Unidad + "  ");
                        hoja.GetRow(row).CreateCell(3).SetCellValue(" " );
                        hoja.GetRow(row).CreateCell(4).SetCellValue(" " + col.Actividad + " ");
                        hoja.GetRow(row).CreateCell(5).SetCellValue(" ");
                        hoja.GetRow(row).CreateCell(6).SetCellValue((double)col.Horas);
                        row++;
                    }                                      

                    // se establecen la longitud de las columnas             
                    for (int i = 0; i < 7; i++) { hoja.AutoSizeColumn(i); }

                    documentoExcel.Write(streamDocumentoExcel);
                    streamDocumentoExcel.Close();
                }
            }
            catch (Exception ex)
            {
                if (streamDocumentoExcel != null) { streamDocumentoExcel.Close(); }

                throw ex;
                resp = -1;
            }
            return resp;
        }

        public int ExportReporteSemanal(string path, List<ReporteSemanal> listaReporte, string encabezado, string subencabezado)
        {
            var resp = 1;
            FileStream streamDocumentoExcel = null;
            try
            {
                List<ReporteSemanal> Semanal = (from l in listaReporte where l.IdProyecto > 0 select l).ToList();
                var ActividadesExt = (from l in listaReporte where l.IdProyecto == 0 select l).ToList();

                using (streamDocumentoExcel = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook documentoExcel = new XSSFWorkbook();
                    ISheet hoja = documentoExcel.CreateSheet("Reporte Semanal " + DateTime.Today.ToString("dd - MM - yyyy"));
                    ICreationHelper cH = documentoExcel.GetCreationHelper();

                    // establecemos los parámetros básicos del documento excel
                    NPOI.HPSF.SummaryInformation si = NPOI.HPSF.PropertySetFactory.CreateSummaryInformation();
                    si.Author = "Bitacora";
                    si.CreateDateTime = DateTime.Now;
                    si.Title = "Reporte";

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont letraNegritaBlanco = documentoExcel.CreateFont();
                    letraNegritaBlanco.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    letraNegritaBlanco.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    letraNegritaBlanco.FontHeightInPoints = (short)12;

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont NegritaBlanca11 = documentoExcel.CreateFont();
                    NegritaBlanca11.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    NegritaBlanca11.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    NegritaBlanca11.FontHeightInPoints = (short)14;

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont letraNegrita = documentoExcel.CreateFont();
                    letraNegrita.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    letraNegrita.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    letraNegrita.FontHeightInPoints = (short)10;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle verde = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    verde.SetFont(letraNegrita);
                    verde.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    verde.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey40Percent.Index;
                    verde.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    verde.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    verde.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Grey50Percent.Index;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTituloAzulF = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTituloAzulF.SetFont(NegritaBlanca11);
                    celdaTituloAzulF.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    celdaTituloAzulF.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                    celdaTituloAzulF.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTituloAzul = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTituloAzul.SetFont(letraNegritaBlanco);
                    celdaTituloAzul.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    celdaTituloAzul.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
                    celdaTituloAzul.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTextoNormal = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTextoNormal.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                    hoja.CreateRow(0);
                    hoja.CreateRow(1);
                    for (int i = 0; i < 6; i++) { hoja.GetRow(0).CreateCell(i).CellStyle = celdaTituloAzulF; hoja.GetRow(1).CreateCell(i).CellStyle = celdaTituloAzul; }
                    hoja.GetRow(0).GetCell(0).SetCellValue(encabezado);
                    hoja.GetRow(1).GetCell(0).SetCellValue(subencabezado);
                    var cra = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 5);
                    var craa = new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 5);
                    hoja.AddMergedRegion(cra);
                    hoja.AddMergedRegion(craa);

                    // creamos la fila para los titulos
                    hoja.CreateRow(2);

                    // se aplica el estilo a las celdas de titulo
                    for (int i = 0; i < 6; i++) { hoja.GetRow(2).CreateCell(i).CellStyle = verde; }

                    // creamos los titulos de las columnas  
                    hoja.GetRow(2).GetCell(0).SetCellValue(" USUARIO ");
                    hoja.GetRow(2).GetCell(1).SetCellValue(" NOMBRE DEL PROYECTO ");
                    hoja.GetRow(2).GetCell(2).SetCellValue(" UNIDAD DE NEGOCIO ");
                    hoja.GetRow(2).GetCell(3).SetCellValue(" AREA DE NEGOCIO ");
                    hoja.GetRow(2).GetCell(4).SetCellValue(" ACTIVIDAD EXTRAORDINARIA ");                    
                    hoja.GetRow(2).GetCell(5).SetCellValue(" HORAS ");

                    // insertamos los datos
                    int row = 3;
                   
                    foreach (var col in Semanal)
                    {
                        hoja.CreateRow(row);
                        hoja.GetRow(row).CreateCell(0).SetCellValue(" " + col.Usuario + " ");
                        hoja.GetRow(row).CreateCell(1).SetCellValue(" " + col.Proyecto + " ");
                        hoja.GetRow(row).CreateCell(4).SetCellValue("");
                        hoja.GetRow(row).CreateCell(2).SetCellValue(" " + col.Unidad + " ");
                        hoja.GetRow(row).CreateCell(3).SetCellValue(" " + col.Area + " ");
                        hoja.GetRow(row).CreateCell(5).SetCellValue((double) col.Horas);
                        row++;
                    }

                    foreach (var col in ActividadesExt)
                    {
                        hoja.CreateRow(row);
                        hoja.GetRow(row).CreateCell(0).SetCellValue(" " + col.Usuario + "  ");
                        hoja.GetRow(row).CreateCell(1).SetCellValue("");
                        hoja.GetRow(row).CreateCell(2).SetCellValue(" ");
                        hoja.GetRow(row).CreateCell(3).SetCellValue(" " + col.Unidad + "  ");
                        hoja.GetRow(row).CreateCell(4).SetCellValue(" " + col.Actividad + " "); 
                        hoja.GetRow(row).CreateCell(5).SetCellValue((double) col.Horas);

                        row++;
                    }


                    // se establecen la longitud de las columnas             
                    for (int i = 0; i < 6; i++) { hoja.AutoSizeColumn(i); }

                    documentoExcel.Write(streamDocumentoExcel);
                    streamDocumentoExcel.Close();
                }
            }
            catch (Exception ex)
            {
                if (streamDocumentoExcel != null) { streamDocumentoExcel.Close(); }

                throw ex;
                resp = -1;
            }
            return resp;
        }

        public int ExportReporteEjecutivol(string path, List<ReporteSemanal> listaReporte, string encabezado, string subencabezado)
        {
            var resp = 1;
            FileStream streamDocumentoExcel = null;
            try
            {

                using (streamDocumentoExcel = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook documentoExcel = new XSSFWorkbook();
                    ISheet hoja = documentoExcel.CreateSheet("Reporte Ejecutivo " + DateTime.Today.ToString("dd - MM - yyyy"));
                    ICreationHelper cH = documentoExcel.GetCreationHelper();

                    // establecemos los parámetros básicos del documento excel
                    NPOI.HPSF.SummaryInformation si = NPOI.HPSF.PropertySetFactory.CreateSummaryInformation();
                    si.Author = "Bitacora";
                    si.CreateDateTime = DateTime.Now;
                    si.Title = "Reporte";

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont letraNegritaBlanco = documentoExcel.CreateFont();
                    letraNegritaBlanco.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    letraNegritaBlanco.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    letraNegritaBlanco.FontHeightInPoints = (short)12;

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont NegritaBlanca11 = documentoExcel.CreateFont();
                    NegritaBlanca11.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    NegritaBlanca11.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    NegritaBlanca11.FontHeightInPoints = (short)14;

                    // estilos de los textos
                    NPOI.SS.UserModel.IFont letraNegrita = documentoExcel.CreateFont();
                    letraNegrita.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    letraNegrita.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    letraNegrita.FontHeightInPoints = (short)10;

                    NPOI.SS.UserModel.IFont letraCuerpo = documentoExcel.CreateFont();
                    letraCuerpo.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    letraCuerpo.FontHeightInPoints = (short)11;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle verde = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    verde.SetFont(letraNegrita);
                    verde.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    verde.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey40Percent.Index;
                    verde.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    verde.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    verde.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Grey50Percent.Index;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTituloAzulF = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTituloAzulF.SetFont(NegritaBlanca11);
                    celdaTituloAzulF.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    celdaTituloAzulF.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                    celdaTituloAzulF.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                    // estilos de las celdas
                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTituloAzul = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTituloAzul.SetFont(letraNegritaBlanco);
                    celdaTituloAzul.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    celdaTituloAzul.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
                    celdaTituloAzul.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTextoNormalCenter = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTextoNormalCenter.SetFont(letraCuerpo);
                    celdaTextoNormalCenter.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                    NPOI.XSSF.UserModel.XSSFCellStyle celdaTextoNormal = (NPOI.XSSF.UserModel.XSSFCellStyle)documentoExcel.CreateCellStyle();
                    celdaTextoNormal.SetFont(letraCuerpo);
                    celdaTextoNormal.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;

                    hoja.CreateRow(0);
                    hoja.CreateRow(1);
                    for (int i = 0; i < 3; i++) { hoja.GetRow(0).CreateCell(i).CellStyle = celdaTituloAzulF; hoja.GetRow(1).CreateCell(i).CellStyle = celdaTituloAzul; }
                    hoja.GetRow(0).GetCell(0).SetCellValue(encabezado);
                    hoja.GetRow(1).GetCell(0).SetCellValue(subencabezado);
                    var cra = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 2);
                    var craa = new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 2);
                    hoja.AddMergedRegion(cra);
                    hoja.AddMergedRegion(craa);

                    // creamos la fila para los titulos
                    hoja.CreateRow(2);

                    // se aplica el estilo a las celdas de titulo
                    for (int i = 0; i < 3; i++) { hoja.GetRow(2).CreateCell(i).CellStyle = verde; }

                    // creamos los titulos de las columnas  
                    hoja.GetRow(2).GetCell(0).SetCellValue(" HORAS ASIGNADAS A: ");
                    hoja.GetRow(2).GetCell(1).SetCellValue(" HORAS ");
                    hoja.GetRow(2).GetCell(2).SetCellValue(" % ");

                    // insertamos los datos
                    int row = 3;

                    foreach (var col in listaReporte)
                    {
                        hoja.CreateRow(row);

                        hoja.GetRow(row).CreateCell(0).CellStyle = celdaTextoNormal;
                        hoja.GetRow(row).CreateCell(1).CellStyle = celdaTextoNormalCenter;
                        hoja.GetRow(row).CreateCell(2).CellStyle = celdaTextoNormalCenter;

                        hoja.GetRow(row).GetCell(0).SetCellValue(" " + col.Proyecto + " ");
                        hoja.GetRow(row).GetCell(1).SetCellValue((double)col.Horas);
                        hoja.GetRow(row).GetCell(2).SetCellValue((int)col.Porcentaje);
                        row++;
                    }

                    hoja.SetColumnWidth(0, 15438);
                    hoja.SetColumnWidth(1, 3456);
                    hoja.SetColumnWidth(2, 3456);

                    documentoExcel.Write(streamDocumentoExcel);
                    streamDocumentoExcel.Close();
                }
            }
            catch (Exception ex)
            {
                if (streamDocumentoExcel != null) { streamDocumentoExcel.Close(); }

                throw ex;
                resp = -1;
            }
            return resp;
        }
    }
}
