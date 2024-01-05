var app = angular.module("app", []);

app.controller("myController", function ($scope, $http) {

    var primerDiaMes = moment().startOf('month').format("DD/MM/YYYY");
    var ultimoDiaMes = new moment().startOf('month').add(1, 'months').add(-1, 'days').format("DD/MM/YYYY");
    var hoy = moment().format("DD/MM/YYYY");
    var parametros = {
        fechadesde: primerDiaMes,
        fechahasta: ultimoDiaMes,
        nombre: "",
        dni: "",
        destino: "",
        encargado: ""
    };

    // $("#fecha").val(hoy);
    $("#fechadesde").val(primerDiaMes);
    $("#fechahasta").val(ultimoDiaMes);
    $("#nombre").val("");
    $("#dni").val("");
    $("#destino").val("");
    $("#encargado").val("");


    /****************EXTRAS MODAL**********************************/
    /**----------------------------------------------------------**/
    $scope.pushGuardar = function () {
        $scope.btntext = "REGISTRAR";
        $scope.inicializar_variables();
    };


    $scope.pushEditar = function (cita) {
        $scope.btntext = "GUARDAR";
        $scope.cita_mod = cita;
    };

    $scope.pushOcultar = function (cita) {
        $scope.cita_mod = cita;
    };



    $scope.pushAnular = function (cita) {
        $scope.cita_mod = cita;
    };

    $scope.pushConfirmar = function (cita) {
        $scope.cita_mod = cita;
        $scope.cita_mod.nroasignado = 0;
        $scope.inicializar_variables();

    };

    /*******************************************************/
    $scope.cerrarModalRegistrar = function () {
        $('#modalAdd').modal('hide');
    };


    $scope.cerrarModalEditar = function () {
        $('#modalEdit').modal('hide');
    };

    $scope.cerrarModalOcultar = function () {
        $('#modalOcultar').modal('hide');
    };

    $scope.cerrarModalAnular = function () {
        $('#modalAnular').modal('hide');
        //jquery->https://www.w3schools.com/bootstrap/bootstrap_ref_js_modal.asp
    };

    $scope.cerrarModalConfirmar = function () {
        $('#modalConfirmar').modal('hide');
    };

    $scope.refrescar = function () {
        $http.get("/Cita/refresh").then(function (d) {
            $scope.refresco = d.data;
        }, function (error) {
            alert('Failed');
        });
    };


    /****************fin MODAL**********************************/

    /********************inicializar variables****************************/
    $scope.inicializar_variables = function () {
        $scope.loadcita(0);

    };
    /**********************************************************************/
    $scope.DisableElementos = function (idperfil) {
        if (idperfil == 2) {
            $("#isdisabledarea").attr("disabled",true);
            entro = "SEGURIDAD";
        }
        //alert("IDPERFIL:" + idperfil + "///ENTRO A:" + entro);
    };


    // Display cita by id***********************
    $scope.loadcita = function (id) {
        $http.get("/Cita/Get_citabyid?id=" + id).then(function (d) {
            $scope.cita = d.data[0];
        }, function (error) {
            alert('Failed');
        });
    };
    /**************Acceso a Base de Datos**********************************/
    /*------------------------------------------------------------*/
    // Display all citas**************************
    $http.get("/Cita/Get_citasDiaActual").
        then(function (d) {
            $scope.citas = d.data;
            $scope.numcitashoy = d.data.length;
            //////////////////////////////////
            $scope.citatemp = d.data[0];
           var idperfil = $scope.citatemp.idperfil;
           $scope.DisableElementos(idperfil);
        }, function (error) {
            alert('conexión fallida!!');
        });


    $http.get('/Cita/Get_citasxFiltros', { params: parametros }).
        then(function (d) {
            $scope.citasfiltradas = d.data;
            $scope.numcitas = d.data.length;
        }, function (error) {
            alert('conexión fallida!!');
        });


    //load list visitas*************************************
    $scope.loadListCitas = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.citas = d.data;
                $scope.numcitasshoy = d.data.length;
                /////////////////////////
                $scope.citatemp = d.data[0];
                var idperfil = $scope.citatemp.idperfil;
                $scope.DisableElementos(idperfil);
               
            }, function (error) {
                alert('conexión fallida!!');
            });
    };



    $scope.filtrarxCampos = function () {
        var f1 = $('#fechadesde').val(); 
        var f2 = $('#fechahasta').val();
        var nombre = $('#nombre').val();
        var dni = $('#dni').val();
        var destino = $('#destino').val();
        var encargado = $('#encargado').val();

        var params = {
            fechadesde: f1,
            fechahasta: f2,
            nombre: nombre,
            dni: dni,
            destino: destino,
            encargado: encargado
        };

        $http.get('/Cita/Get_citasxFiltros', { params: params }).
            then(function (d) {
                $scope.citasfiltradas = d.data;
                $scope.numcitas = d.data.length;
                alert("Búsqueda realizada!!");

            }, function (error) {
                alert('conexión fallida!!');
            });

    };




    // Add visita*************************************
    $scope.savecita = function (cita) {
        $scope.cita = cita;

        var url_data = "/Cita/Get_citasDiaActual";
        $.ajax({
            type: "POST",
            url: "/Cita/registrar_cita",
            data: $scope.cita,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "SI" || response == "NO") {
                        if (response == "SI") {
                            alert("cita registrada!!");
                            $scope.cerrarModalRegistrar();
                            $scope.loadListCitas(url_data);
                        }

                        if (response == "NO") {
                            alert("El citado ya tiene una cita agendada con los mismos datos!!");
                            $scope.btntext = "REGISTRAR";
                            $scope.refrescar();
                        }
                    }
                    else {
                        alert(response);
                        $scope.btntext = "REGISTRAR";
                        $scope.refrescar();
                    }

                } else {
                    alert("registro fallido!!");
                }

            },
            dataType: 'json'
        });

    };


    /**************************************************************/
    // Update cita*****************************
    $scope.updatecita = function () {

        $scope.btntext = "Please Wait..";
        var url_data = "/Cita/Get_citasDiaActual";

        $.ajax({
            type: "POST",
            url: "/cita/actualizar_cita",
            data: $scope.cita_mod,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "SI" || response == "NO") {
                        if (response == "SI") {
                            alert("visitante actualizado!!");
                            $scope.cerrarModalEditar();
                            $scope.loadListCitas(url_data);
                        }

                        if (response == "NO") {
                            alert("El citado ya tiene una cita agendada con los mismos datos!!");
                            $scope.btntext = "GUARDAR";
                            $scope.refrescar();
                        }
                    }
                    else {
                        alert(response);
                        $scope.btntext = "GUARDAR";
                        $scope.refrescar();
                    }

                } else {
                    alert("registro fallido!!");
                }
            },
            dataType: 'json'
        });

    };


    // ocultar cita*****************************
    $scope.ocultarcita = function () {

        $scope.btntext = "Please Wait..";
        var url_data = "/Cita/Get_citasDiaActual";

        $.ajax({
            type: "POST",
            url: "/cita/ocultar_cita",
            data: $scope.cita_mod,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "SI" || response == "NO") {
                        if (response == "SI") {
                            alert("cita ocultada!!");
                            $scope.cerrarModalAnular();
                            $scope.loadListCitas(url_data);
                        }

                        if (response == "NO") {
                            alert("No se puede ocultar la cita seleccionada!!");
                            $scope.refrescar();
                        }
                    }
                    else {
                        alert(response);
                        $scope.refrescar();
                    }

                } else {
                    alert("registro fallido!!");
                }
            },
            dataType: 'json'
        });

    };

    /****************************************************************/
    // ocultar cita*****************************
    $scope.anularcita = function () {

        $scope.btntext = "Please Wait..";
        var url_data = "/Cita/Get_citasDiaActual";

        $.ajax({
            type: "POST",
            url: "/cita/anular_cita",
            data: $scope.cita_mod,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "SI" || response == "NO") {
                        if (response == "SI") {
                            alert("cita anulada!!");
                            $scope.cerrarModalOcultar();
                            $scope.loadListCitas(url_data);
                        }

                        if (response == "NO") {
                            alert("No se puede anular la cita seleccionada!!");
                            $scope.refrescar();
                        }
                    }
                    else {
                        alert(response);
                        $scope.refrescar();
                    }

                } else {
                    alert("registro fallido!!");
                }
            },
            dataType: 'json'
        });

    };
    /****************************************************************/
    // ocultar cita*****************************
    $scope.confirmarvisita = function () {
        $scope.cita_mod.hora = $scope.cita.hora;
        $scope.btntext = "Please Wait..";
        var url_data = "/Cita/Get_citasDiaActual";

        $.ajax({
            type: "POST",
            url: "/cita/confirmar_visita",
            data: $scope.cita_mod,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "SI" || response == "NO") {
                        if (response == "SI") {
                            alert("Asistencia confirmada!!");
                            $scope.cerrarModalConfirmar();
                            $scope.loadListCitas(url_data);
                        }

                        if (response == "NO") {
                            alert("El citado ya tiene ingreso o EL número ya está asignado, primero registre la salida del visitante o número para registrar su ingreso de nuevo!!");
                            $scope.refrescar();
                        }
                    }
                    else {
                        alert(response);
                        $scope.refrescar();
                    }

                } else {
                    alert("registro fallido!!");
                }
            },
            dataType: 'json'
        });

    };



   

    //***********************************************************/
    $scope.reporteDelDia = function () {

        window.location.replace("/Reportes/ReporteCitasHoy");

    };
    //**********************************************************//
    $scope.reporteFiltros = function () {
        var url_data = "/Reportes/ReporteCitasFiltros";
        var f1 = $('#fechadesde').val();
        var f2 = $('#fechahasta').val();
        var nombre = $('#nombre').val();
        var dni = $('#dni').val();
        var destino = $('#destino').val();
        var encargado = $('#encargado').val();

        /******************************** */
        window.location = url_data +
            "?fechadesde=" + f1 +
            "&fechahasta=" + f2 +
            "&nombre=" + nombre +
            "&dni=" + dni +
             "&encargado=" + encargado +
            "&destino=" + destino 
           
        ;
    };
    /*************************************************************/




});