

//**********FILTROS BÚSQUEDAS SIMPLES EN TODAS LAS COMUNAS****************/
jQuery("#filtro").keyup(function () {
    if (jQuery(this).val() != "") {
        jQuery("#tablaDatos tbody>tr").hide();
        jQuery("#tablaDatos td:contiene-palabra('" + jQuery(this).val() + "')").parent("tr").show();
    }
    else {
        jQuery("#tablaDatos tbody>tr").show();
    }
});



jQuery.extend(jQuery.expr[":"],
{
    "contiene-palabra": function (elem, i, match, array) {
        return (elem.textContent || elem.innerText || jQuery(elem).text() || "").toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0;
    }
    });


$("#filtroModal").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    $("#tablaDatosModal tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
});


///---------------validacióm del dni-------------------////
jQuery("#dni").keyup(function () {
    numDni = $('#dni').val();
    if (numDni.length > 8 || numDni.length < 8) {
        //alert('El valor del dni no es válido!!');
        $('#dni').css({ 'borderColor': 'red', 'borderStyle': 'solid', 'borderWidth': '5px' });
    }
    else {
        $('#dni').css({ 'borderColor': 'green', 'borderStyle': 'solid', 'borderWidth': '5px' });
    }
});



/************Transcribir en el panel de Filtros*****************/
jQuery("#fechai").change(function () {
    var fecha = $('#fechai').val();
    $('#fecha1').val(fecha);
});

jQuery("#fechaf").change(function () {
    var fechatemp = $('#fechaf').val();
    $('#fecha2').val(fechatemp);
});


jQuery("#nombre").keyup(function () {
    var nombretemp = $('#nombre').val();
    $('#nombre2').val(nombretemp);
});

jQuery("#dni").keyup(function () {
    var dnitemp = $('#dni').val();
    $('#dni2').val(dnitemp);
});


jQuery("#destino").keyup(function () {
    var destinotemp = $('#destino').val();
    $('#destino2').val(destinotemp);
});


jQuery("#empresa").keyup(function () {
    var proveedortemp = $('#empresa').val();
    $('#empresa2').val(proveedortemp);
});
/****************solo validar números*************************/
$('.numerico').keyup(function () {
    this.value = (this.value + '').replace(/[^0-9]/g, '');
});

function validaNumericos(event) {
    if (event.charCode >= 48 && event.charCode <= 57) {
        return true;
    }
    return false;
}
/*********************solo campo de lectura***********************/
$(".readonly").focus(function () {
    $(this).blur();
    alert('Campo no editable!!');
});

$(".readonlyfec").focus(function () {
    $(this).blur();
});
/****************CALENDARIO DATE PICKER***********/
//-----------------------------
var today = new Date();
$(".form_datetime").datetimepicker({
    format: 'dd/mm/yyyy',
    autoclose: true,
    todayBtn: true,
    //showMeridian: true,//AM-PM OR FORMATO 24HORAS
    language: 'es',
    //locale: 'es',
    startView: 2,
    minView: 2,
    //forceParse: 0,//se selecciona la fecha actual por defecto
    //startDate: today ,//solo se muestran fechas desde el día actual
    pickerPosition: "top-right"
});

$(".form_datetimefiltro").datetimepicker({
    format: 'dd/mm/yyyy',
    autoclose: true,
    todayBtn: true,
    //showMeridian: true,//AM-PM OR FORMATO 24HORAS
    language: 'es',
    //locale: 'es',
    startView: 2,
    minView: 2,
    //forceParse: 0,//se selecciona la fecha actual por defecto
    //startDate: today ,//solo se muestran fechas desde el día actual
    pickerPosition: "bottom-right"
});

$(".form_datetimeUp").datetimepicker({
    format: 'dd/mm/yyyy',
    autoclose: true,
    todayBtn: true,
    //showMeridian: true,//AM-PM OR FORMATO 24HORAS
    language: 'es',
    //locale: 'es',
    startView: 2,
    minView: 2,
    //forceParse: 0,//se selecciona la fecha actual por defecto
    //startDate: today ,//solo se muestran fechas desde el día actual
    pickerPosition: "top-right"
});

$(".form_datetimeDown").datetimepicker({
    format: 'dd/mm/yyyy',
    autoclose: true,
    todayBtn: true,
    //showMeridian: true,//AM-PM OR FORMATO 24HORAS
    language: 'es',
    //locale: 'es',
    startView: 2,
    minView: 2,
    firstDay: 1,
    //forceParse: 0,//se selecciona la fecha actual por defecto
    startDate: today ,//solo se muestran fechas desde el día actual
    pickerPosition: "bottom-right"
});


$(".form_timeDown").datetimepicker({
    //format: 'hh:mm',
    pickDate: false,
    autoclose: true,
    language: 'es'
});

$(function () {
    $('#form_timeDown').datetimepicker({
        format: 'hh:ii',
        pickDate: false
    });
});

/************* convierte texto a mayuscula******************/
function mayusculas(e) {
    e.value = e.value.toUpperCase();
};



///**********cambiar valor al dar modificar SELECT************/
//jQuery("#idempr").change(function () {
//    var empresa = $('#idempr').val();
    
//    $('#emprSel').val(empresa);
//    alert($('#emprSel').val());
//});
/***********************************/

/**************************************************************/
$('.radioBtn a').on('click', function () {
    var sel = $(this).data('title');
    var tog = $(this).data('toggle');
    $('#' + tog).prop('value', sel);

    $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
    $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');
});

