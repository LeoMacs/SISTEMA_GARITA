var app = angular.module("app", []);


app.controller("myController", function ($scope, $http) {

    var primerDiaMes = moment().startOf('month').format("DD/MM/YYYY");
    var ultimoDiaMes = new moment().startOf('month').add(1, 'months').add(-1, 'days').format("DD/MM/YYYY");
    //$scope.numasistenciashoy = 0;
    var parametros = {
        fechadesde: primerDiaMes,
        fechahasta: ultimoDiaMes,
        nombre: "",
        dni: "",
        empresa: "",
       
    };

    $("#fechadesde").val(primerDiaMes);
    $("#fechahasta").val(ultimoDiaMes);
    $("#nombre").val("");
    $("#dni").val("");
    $("#empresa").val("");

    /****************EXTRAS MODAL**********************************/

    $scope.pushTrabajadores = function () {

        var url_data = "/Asistencia/Get_trabajadores";
        $scope.loadListTrabajadores(url_data);
    };

    $scope.pushConfirmarSalida = function (asistencia) {
        $scope.asistencia = asistencia;
    };

    $scope.cerrarModalTrabajadores = function () {
        $('#modalTrabajadores').modal('hide');
    };

    $scope.cerrarModalSalida = function () {
        $('#modalSalida').modal('hide');
    };

    $scope.refrescar = function () {
        $http.get("/Asistencia/refresh").then(function (d) {
            $scope.refresco = d.data;
        }, function (error) {
            alert('Failed');
        });
    };

    /************************************************************************************/
    /*----------------------------------------------------------------------------------*/

    //////**************************** Mostrar todas las asistencias*****************


    $http.get("/Asistencia/Get_asistenciasHoy").then(function (d) {
        $scope.asistencias = d.data;
        $scope.numasistenciashoy = d.data.length;

    }, function (error) {
        alert('Failed');
    });


    // Mostrar asistencias***********************
    $scope.loadListTrabajadores = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.trabajadores = d.data;

            }, function (error) {
                alert('conexión fallida!!');
            });
    };


    // Mostrar asistencias***********************
    $scope.loadListAsistencias = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.asistencias = d.data;
                $scope.numasistenciashoy = d.data.length;

            }, function (error) {
                alert('conexión fallida!!');
            });
    };



    ///****************Obtener empresa*******************************///
    $scope.loadasistencia = function (asistencia) {

        $http.get("/Asistencia/Get_asistenciabyid?id=" + asistencia.idasistencia).then(function (d) {
            $scope.asistencia = d.data[0];
        }, function (error) {
            alert('Failed');
        });

    };

    ///****************Guardar empresa*******************************///
    $scope.saveasistencia = function (trabajador) {
        $scope.trabajador = trabajador;

        var url_data = "/Asistencia/Get_asistenciasHoy";

        $.ajax({
            type: "POST",
            url: "/Asistencia/Add_asistencia",
            data: $scope.trabajador,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "S" || response == "N") {

                        if (response == "S") {
                            alert("asistencia registrada!!");
                            $scope.cerrarModalTrabajadores();
                            $scope.loadListAsistencias(url_data);
                        }

                        if (response == "N") {
                            alert("Error: primero registre la salida del trabajador para registrar su ingreso de nuevo!!");
                            $scope.refrescar();

                        }
                    }
                    else {
                        alert(response);

                    }


                } else {
                    alert("registro fallido!!");
                }
            },
            dataType: 'json'
        });

    };



    ///****************Editar asistencia*******************************///
    $scope.savesalida = function () {
        var url_data = "/Asistencia/Get_asistenciasHoy";

        $.ajax({
            type: "POST",
            url: '/Asistencia/Update_asistencia',
            data: $scope.asistencia,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {
                    /********************************************/
                    if (response == "S" || response == "N") {


                        if (response == "S") {
                            alert("Salida registrada!!");
                            $scope.cerrarModalSalida();
                            $scope.loadListAsistencias(url_data);
                        }

                        if (response == "N") {
                            alert("Error al registrar la salida!!");
                            $scope.cerrarModalSalida();

                        }


                    }
                    else {
                        $scope.refrescar();
                        $scope.cerrarModalSalida();

                    }

                    /********************************************/


                } else {
                    alert("registro de salida fallida!!");
                }
            },
            dataType: 'json'
        });

    };
    /**************************************************************/
    /**************************************************************/

    $http.get("/Asistencia/Get_asistenciasxFiltros", { params: parametros }).then(function (d) {
        $scope.asistenciasfiltradas = d.data;
        $scope.numasistencias = d.data.length;

    }, function (error) {
        alert('Failed');
    });

    /***************************************************************/
    $scope.filtrarxCampos = function () {

        var f1 = $('#fechadesde').val();
        var f2 = $('#fechahasta').val();
        var nombre = $('#nombre').val();
        var dni = $('#dni').val();
        var empresa = $('#empresa').val();

        var parametrs = {
            fechadesde: f1,
            fechahasta: f2,
            nombre: nombre,
            dni: dni,
            empresa:empresa
        };

        $http.get('/Asistencia/Get_asistenciasxFiltros', { params: parametrs}).
                   then(function (d) {
                       $scope.asistenciasfiltradas = d.data;
                       $scope.numasistencias = d.data.length;

                       alert("Búsqueda realizada!!");

                   }, function (error) {
                       alert('conexión fallida!!');
                   });
    };

    /***************************************************************/

    $scope.reporteDelDia = function () {
        
        window.location.replace("/Reportes/ReporteAsistenciasHoy");

    };

    $scope.reporteFiltros = function () {
        var url_data = "/Reportes/ReporteAsistenciasFiltros";
        var fechai = $("#fechadesde").val();
        var fechaf = $("#fechahasta").val();
        var nombre = $("#nombre").val();
        var dni = $("#dni").val();
        var empresa = $("#empresa").val();
        /******************************** */
        window.location=url_data +
            "?fechai=" +fechai +
            "&fechaf=" + fechaf +
            "&nombre=" + nombre +
            "&dni=" + dni +
            "&empresa=" + empresa ;                
    };


   

    /*****************************************************************/
   

});
