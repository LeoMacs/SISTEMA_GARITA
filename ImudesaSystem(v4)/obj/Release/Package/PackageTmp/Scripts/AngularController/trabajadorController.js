var app = angular.module("app", []);


app.controller("myController", function ($scope, $http) {

    /****************EXTRAS MODAL**********************************/
    $scope.pushGuardar = function () {
        $scope.btntext = "REGISTRAR";
        $scope.trabajador = null;
        var url_data = "/Trabajador/Get_empresas";
        $scope.loadListempresas(url_data);
        //$scope.trabajador.idempresa = 0;
    };

    $scope.pushEditar = function (trabajador) {
        
        $scope.btntext = "GUARDAR";
        $scope.loadtrabajador(trabajador);
        var activo = trabajador.habilitado;
        var conSeguro = trabajador.conSeguro;
        
        var url_data = "/Trabajador/Get_empresasconSeleccion?idempresa=" + trabajador.idempresa;
        $scope.loadListempresas(url_data);
        //var fechai = $("#fecha1").val();

        if (activo == 1) {
            $("#sw_lang_left").prop('checked', true).attr('checked', 'checked');//boton si
            $("#sw_lang_right").prop('checked', false).removeAttr('checked');//boton no
        } else {
            $("#sw_lang_right").prop('checked', true).attr('checked', 'checked');//boton si
            $("#sw_lang_left").prop('checked', false).removeAttr('checked');//boton no
        }
        //alert('activo:'+activo);
        if (conSeguro == "SI") {
            $("#sw_lang_left1").prop('checked', true).attr('checked', 'checked');//boton si
            $("#sw_lang_right1").prop('checked', false).removeAttr('checked');//boton no
        } else {
            $("#sw_lang_right1").prop('checked', true).attr('checked', 'checked');//boton si
            $("#sw_lang_left1").prop('checked', false).removeAttr('checked');//boton no
        }
       

    };


    $scope.cerrarModalAgregar = function () {
        $('#modalAdd').modal('hide');
    };

    $scope.cerrarModalEditar = function () {
        $('#modalEdit').modal('hide');
    };


    $scope.actBtn = function (num) {
        $http.get("/Trabajador/refresh").then(function (d) {
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


    ////**************************** DataBase trabajadores*****************
    $scope.loadListTrabajadores = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.trabajadores = d.data;
                $scope.numtrabajadores = d.data.length;

            }, function (error) {
                alert('conexión fallida!!');
            });
    };


    $http.get("/Trabajador/Get_trabajadores").then(function (d) {
        $scope.trabajadores = d.data;
        $scope.numtrabajadores = d.data.length;

    }, function (error) {
        alert('Failed');
    });


    $scope.loadListempresas = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.empresas = d.data;
            }, function (error) {
                alert('conexión fallida!!');
            });
    };


    $scope.loadtrabajador = function (trabajador) {
        $http.get("/Trabajador/Get_trabajadorbyid?id=" + trabajador.idtrabajador).then(function (d) {
            $scope.trabajador = d.data[0];
        }, function (error) {
            alert('Failed');
        });
    };

    ///****************Guardar trabajador*******************************///
    $scope.savetrabajador = function () {

        $scope.btntext = "Please Wait..";
        var url_data = "/Trabajador/Get_trabajadores";
        var idempresa = $("#idempr").val();
        $scope.trabajador.idempresa = idempresa;

        $.ajax({
            type: "POST",
            url: "/Trabajador/Add_Trabajador",
            data: $scope.trabajador,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "S" || response == "N") {

                        if (response == "S") {
                            alert("trabajador registrado!!");
                            $scope.cerrarModalAgregar();
                            $scope.loadListTrabajadores(url_data);
                        }

                        if (response == "N") {
                            alert("trabajador ya registrado antes!!");
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
    
    ///****************actualizar trabajador*******************************///
    $scope.updatetrabajador = function () {

        $scope.btntext = "Please Wait..";
        var url_data = "/Trabajador/Get_trabajadores";
        var idempresa = $("#idempresa").val();

        if ($("#sw_lang_left").is(':checked')) {
            $scope.trabajador.habilitado = 1;
        } else {
            $scope.trabajador.habilitado = 0;
        }

        if ($("#sw_lang_left1").is(':checked')) {
            $scope.trabajador.conSeguro = "SI";
        } else {
            $scope.trabajador.conSeguro = "NO";
        }

        $scope.trabajador.idempresa = idempresa;

        $.ajax({
            type: "POST",
            url: "/Trabajador/update_trabajador",
            data: $scope.trabajador,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "S" || response == "N") {

                        if (response == "S") {
                            alert("trabajador actualizado!!");
                            $scope.cerrarModalEditar();
                            $scope.loadListTrabajadores(url_data);
                        }

                        if (response == "N") {
                            alert("falló al actualizar trabajador!!");
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
    
    $scope.reporteTrabajadores = function () {

        window.location.replace("/Reportes/reporteTrabajadores");

    };


   




});
