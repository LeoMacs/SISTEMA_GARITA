
///////////////////////FILTRO POR DNI/////////////////////////////////////
jQuery("#dni").keyup(function () {
    if (jQuery(this).val() != "") {
        jQuery("#tablaDatos tbody>tr").hide();
        jQuery("#tablaDatos td:contiene-palabra('" + jQuery(this).val() + "')").parent("tr").show();
    }

});

jQuery.extend(jQuery.expr[":"],
{
    "contiene-palabra": function (elem, i, match, array) {
        return (elem.textContent || elem.innerText || jQuery(elem).text() || "").toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0;
    }
});


//////////////////FILTRO POR NOMBRE//////////////////
jQuery("#nombre").keyup(function () {
    if (jQuery(this).val() != "") {
        jQuery("#tablaDatos tbody>tr").hide();
        jQuery("#tablaDatos td:contiene-palabra('" + jQuery(this).val() + "')").parent("tr").show();
    }

});

jQuery.extend(jQuery.expr[":"],
{
    "contiene-palabra": function (elem, i, match, array) {
        return (elem.textContent || elem.innerText || jQuery(elem).text() || "").toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0;
    }
});



//////////////////FILTRO POR destino//////////////////
$(document).ready(function () {
    (function ($) {
        $('#destino').keyup(function () {
            var rex = new RegExp($(this).val(), 'i');
            $('#TablaBody tr').hide();
            $('#TablaBody tr').filter(function () {
                return rex.test($(this).text());
            }).show();
        })
    }(jQuery));

});

//////////////////FILTRO POR proveedor//////////////////

$(document).ready(function () {
    (function ($) {
        $('#proveedor').keyup(function () {
            var rex = new RegExp($(this).val(), 'i');
            $('#TablaBody tr').hide();
            $('#TablaBody tr').filter(function () {
                return rex.test($(this).text());
            }).show();
        })
    }(jQuery));

});



function jsBuscar() {
    //obtenemos el valor insertado a buscar
    //buscar = $("#proveedor").prop("value")


    //realizamos el recorrido solo por las celdas que contienen el código, que es la primera
    //$("#tablaDatos tr").find('td:eq(8)').each(function () {

       

        //comparamos para ver si el código es igual a la busqueda
        //if (proveedor == buscar) {

            //aqui ya que tenemos el td que contiene el codigo utilizaremos parent para obtener el tr.
            var rex = new RegExp($(this).val(), 'i');
            $('#TablaBody tr').filter(function () {
                return rex.test($(this).text());
            }).show();
           

       // }

    //})  
}

/**********************************************************/
/**********************************************************/

//function jsBuscar() {
//    //obtenemos el valor insertado a buscar
//    buscar = $("#proveedor").prop("value")

//    //utilizamos esta variable solo de ayuda y mostrar que se encontro
//    encontradoResultado = false;

//    //realizamos el recorrido solo por las celdas que contienen el código, que es la primera
//    $("#tablaDatos tr").find('td:eq(0)').each(function () {

//        //obtenemos el codigo de la celda
//        codigo = $(this).html();

//        //comparamos para ver si el código es igual a la busqueda
//        if (codigo == buscar) {

//            //aqui ya que tenemos el td que contiene el codigo utilizaremos parent para obtener el tr.
//            trDelResultado = $(this).parent();

//            //ya que tenemos el tr seleccionado ahora podemos navegar a las otras celdas con find
//            nombre = trDelResultado.find("td:eq(1)").html();
//            edad = trDelResultado.find("td:eq(2)").html();

//            //mostramos el resultado en el div
//            $("#mostrarResultado").html("El nombre es: " + nombre + ", la edad es: " + edad)

//            encontradoResultado = true;

//        }

//    })

//    //si no se encontro resultado mostramos que no existe.
//    if (!encontradoResultado)
//        $("#mostrarResultado").html("No existe el código: " + buscar)
//}

/**********************************************************/
/**********************************************************/



/*******************Estilos para tabla*********************/
/**********************************************************/
/*función para inicializar tablas con par e impar*/
//jQuery(document).ready(function () {   
//    filas_cebra('tbody tr:odd', 'impar');
//});

////función para aplicar la clase
//function filas_cebra(selector, clase) {
//    jQuery(selector).removeClass(clase).addClass(clase);
//}