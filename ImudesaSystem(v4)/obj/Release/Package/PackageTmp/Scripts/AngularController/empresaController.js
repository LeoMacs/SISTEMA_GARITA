var app = angular.module("app", []);


app.controller("myController", function ($scope, $http) {


    /****************EXTRAS MODAL**********************************/
    $scope.pushPersonal = function (empresa) {
        var idempresa = empresa.idempresa;
        $scope.nombreEmpresa = empresa.nombre;
        var url_data = "/Empresa/Get_Personalxempresa?id=" + idempresa;
        $scope.loadListTrabajadores(url_data);

    };

    $scope.pushSctr = function (empresa) {
        $scope.nombreEmpresa = empresa.nombre;
        $scope.loadseguro(empresa.idempresa);
    };

    $scope.pushGuardar = function () {
        $scope.btntext = "REGISTRAR";
        $scope.empresa = null;
    };

    $scope.pushEditar = function (empresa) {
        $scope.btntext = "GUARDAR";
        $scope.loadempresa(empresa);
    };

    $scope.cerrarModalAgregar = function () {
        $('#modalAdd').modal('hide');
    };

    $scope.cerrarModalEditar = function () {
        $('#modalEdit').modal('hide');
    };

    /************************************************************************************/
    /*----------------------------------------------------------------------------------*/

    //////**************************** Mostrar todas las empresas*****************
    $http.get("/Empresa/Get_empresas").then(function (d) {
        $scope.empresas = d.data;
        $scope.numempresas = d.data.length;

    }, function (error) {
        alert('Failed');
    });


    // Mostrar seguro por empresa by id***********************
    $scope.loadseguro = function (id) {
        $http.get("/Empresa/Get_SCTRxempresa?id=" + id).then(function (d) {
            $scope.seguro = d.data[0];
        }, function (error) {
            alert('Failed');
        });
    };

    // Mostrar trabajores por empresa by id***********************
    $scope.loadListTrabajadores = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.trabajadores = d.data;
                $scope.numtrabajadores = d.data.length;
            }, function (error) {
                alert('conexión fallida!!');
            });
    };

    /*******************************************************************/
    //load list visitas*************************************
    $scope.loadListempresas = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.empresas = d.data;
                $scope.numempresas = d.data.length;

            }, function (error) {
                alert('conexión fallida!!');
            });
    };

    ///****************Obtener empresa*******************************///
    $scope.loadempresa = function (empresa) {

        $http.get("/Empresa/Get_empresabyid?id=" + empresa.idempresa).then(function (d) {
            $scope.empresa = d.data[0];
        }, function (error) {
            alert('Failed');
        });

    };

    ///****************Guardar empresa*******************************///
    $scope.saveempresa = function () {

        $scope.btntext = "Please Wait..";
        var url_data = "/Empresa/Get_empresas";

        $.ajax({
            type: "POST",
            url: "/Empresa/Add_empresa",
            data: $scope.empresa,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "S" || response == "N") {

                        if (response == "S") {
                            alert("Empresa registrada!!");
                            $scope.cerrarModalAgregar();
                            $scope.loadListempresas(url_data);
                        }

                        if (response == "N") {
                            alert("Error: Empresa ya registrada!!");
                            $scope.btntext = "REGISTRAR";
                            $scope.loadListempresas(url_data);

                        }
                    }
                    else {
                        $scope.btntext = "REGISTRAR";
                        $scope.loadListempresas(url_data);
                        alert(response);

                    }


                } else {
                    alert("registro fallido!!");
                }
            },
            dataType: 'json'
        });


    };



    ///****************Editar empresa*******************************///
    $scope.updateempresa = function () {
        $scope.btntext = "Please Wait..";
        var url_data = "/Empresa/Get_empresas";

        $.ajax({
            type: "POST",
            url: '/Empresa/update_empresa',
            data: $scope.empresa,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {
                    /********************************************/
                    if (response == "S" || response == "N") {


                        if (response == "S") {
                            alert("Empresa actualizada!!");
                            $scope.cerrarModalEditar();
                            $scope.loadListempresas(url_data);
                        }

                        if (response == "N") {
                            alert("Error al actualizar la empresa!!");
                            $scope.btntext = "GUARDAR";
                            $scope.loadListempresas(url_data);

                        }


                    }
                    else {
                        $scope.btntext = "GUARDAR";
                        $scope.loadListempresas(url_data);
                        alert(response);


                    }

                } else {
                    alert("actualización fallida!!");
                }
            },
            dataType: 'json'
        });

    };

    $scope.reporteempresas = function () {

        window.location.replace("/Reportes/ReporteEmpresas");

    };



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
