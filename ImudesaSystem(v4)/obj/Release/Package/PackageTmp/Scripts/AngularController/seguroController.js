var app = angular.module("app", []);


app.controller("myController", function ($scope, $http) {


    /****************EXTRAS MODAL**********************************/

    $scope.pushPersonal = function (seguro) {
        var idempresa = seguro.idempresa;
        $scope.nombreEmpresa = seguro.empresa;
        var url_data = "/Seguro/get_PersonalxSeguro?id=" + idempresa;
        $scope.loadListTrabajadores(url_data);
    };



    $scope.pushGuardar = function () {
        $scope.btntext = "REGISTRAR";
        $scope.seguro = null;
        var url_data = "/Seguro/Get_empDispSeguro";
        $scope.loadListEmpDisp(url_data);
        $scope.seguro.idempresa = 0;
    };

    $scope.pushEditar = function (seguro) {
        $scope.btntext = "GUARDAR";
        //$scope.loadseguro(seguro);
        $scope.seguro = seguro;
    };

    $scope.pushAnular = function (seguro) {
        $scope.btntext = "DELETE";
        $scope.seguroanulado = seguro;//variable para eliminr
    };

    $scope.cerrarModalAgregar = function () {
        $('#modalAdd').modal('hide');
    };

    $scope.cerrarModalEditar = function () {
        $('#modalEdit').modal('hide');
    };

    $scope.cerrarModalAnular = function () {
        $('#modalAnular').modal('hide');
        //jquery->https://www.w3schools.com/bootstrap/bootstrap_ref_js_modal.asp
    };

    $scope.actBtn = function (num) {
        $http.get("/Seguro/refresh").then(function (d) {
            $scope.refresco = d.data;
        }, function (error) {
            alert('Failed');
        });

        if (num == 1) {
            $scope.btntext = "REGISTRAR";

        }
        else {
            $scope.btntext = "GUARDAR";

        }
    };



    /************************************************************************************/
    /*----------------------------------------------------------------------------------*/

    //////**************************** Mostrar todas las seguros*****************
    $http.get("/Seguro/Get_seguros").then(function (d) {
        $scope.seguros = d.data;
        $scope.numseguros = d.data.length;
    }, function (error) {
        alert('Failed');
    });


    // Mostrar trabajores por seguro by id***********************
    $scope.loadListTrabajadores = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.trabajadores = d.data;
                $scope.numtrabajadores = d.data.length;

            }, function (error) {
                alert('conexión fallida!!');
            });
    };
    /******************Lista de Seguros********************************/
    $scope.loadListseguros = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.seguros = d.data;
                $scope.numseguros = d.data.length;

            }, function (error) {
                alert('conexión fallida!!');
            });
    };

    //***********************LISTA DE EMPRESAS DISPONIBLES SIN SEGURO*******//
    $scope.loadListEmpDisp = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.empresas = d.data;
            }, function (error) {
                alert('conexión fallida!!');
            });
    };

    ///****************Obtener seguro*****************************///
    $scope.loadseguro = function (seguro) {

        $http.get("/Seguro/Get_segurobyid?id=" + seguro.idseguro).then(function (d) {
            $scope.seguro = d.data[0];
        }, function (error) {
            alert('Failed');
        });

    };

    ///****************Guardar seguro*******************************///
    $scope.saveseguro = function () {

        $scope.btntext = "Please Wait..";
        var url_data = "/Seguro/Get_seguros";

        $.ajax({
            type: "POST",
            url: "/Seguro/Add_seguro",
            data: $scope.seguro,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "S" || response == "N") {

                        if (response == "S") {
                            alert("seguro registrado!!");
                            $scope.cerrarModalAgregar();
                            $scope.loadListseguros(url_data);
                        }

                        if (response == "N") {
                            alert("Error de fechas al registrar el seguro!!");
                            $scope.actBtn(1);


                        }
                    }
                    else {
                        alert(response);
                        $scope.actBtn(1);


                    }


                } else {
                    alert("registro fallido!!");
                    $scope.actBtn(1);

                }
            },
            dataType: 'json'
        });


    };



    ///****************Editar seguro*******************************///
    $scope.updateseguro = function () {
        $scope.btntext = "Please Wait..";
        var url_data = "/Seguro/Get_seguros";
        
        $.ajax({
            type: "POST",
            url: '/Seguro/update_seguro',
            data: $scope.seguro,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {
                    /********************************************/
                    if (response == "SI" || response == "NO") {
                        if (response == "SI") {
                            alert("seguro actualizado!!");
                            $scope.cerrarModalEditar();
                            $scope.loadListseguros(url_data);
                        }

                        if (response == "N") {
                            alert("Error de fechas al actualizar el seguro!!");
                            //$scope.btntext = "GUARDAR";
                            //$scope.loadListseguros(url_data);
                            $scope.actBtn(2);
                        }
                    }
                    else {
                        alert(response);
                        $scope.actBtn(2);

                    }

                } else {
                    alert("actualización fallida!!");
                    $scope.actBtn(2);
                }
            },
            dataType: 'json'
        });

    };
    /*********************************************************/
    

    /***********************************************************/

    $scope.anular = function () {
        var url_data = "/Seguro/Get_seguros";
        $.ajax({
            type: "POST",
            url: '/Seguro/anularSeguro',
            data: $scope.seguroanulado,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {
                    /********************************************/
                    if (response == "SI" || response == "NO") {

                        if (response == "SI") {
                            alert("Seguro anulado!!");
                            $scope.cerrarModalAnular();
                            $scope.loadListseguros(url_data);
                        }

                        if (response == "NO") {
                            alert("No se pudo anular el seguro!!");
                            $scope.cerrarModalSalida();
                        }
                    }

                } else {
                    alert(response);
                }
            },
            dataType: 'json'
        });
    };
    /*************************************************************/

    $scope.reporteSeguros = function () {

        window.location.replace("/Reportes/reporteSCTR");

    };





    /***************************************************************/
    $scope.cerrarSesion = function () {

        $http.get("/Home/cerrarSesion").then(function (d) {
            $scope.respuesta = d.responseText;
            if (respuesta == "SI") {
                alert("Sesión cerrada!!");
                window.location.replace("/Home/Login");
            }
        }, function (error) {
            alert('Failed');
        });

    };

    $scope.cerrarSesion = function () {
        var respuesta;
        if (confirm("Está seguro de Cerrar Sesión?")) {
            $http.get("/Home/cerrarSesion").then(function (d) {
                respuesta = d.responseText;
                if (respuesta == "SI") {
                    alert("Sesión cerrada!!");
                    window.location.replace("/Home/Login");
                }
            }, function (error) {
                alert('Failed');
            });
        }

    };





});
