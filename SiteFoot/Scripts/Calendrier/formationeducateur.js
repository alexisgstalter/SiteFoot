$(document).ready(function () {
    $("select").material_select();
    $("#debut ,#fin ,#debut_modif ,#fin_modif").datetimepicker({
        lang: 'fr',
        mask: true,
        format: 'd/m/Y H:i',
        step: 15
    });
    $("select").change(function () {
        $("#calendrier_formation").fullCalendar('refetchEvents');
    });
    var current_id;
    $("#calendrier_formation").fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay',
            lang: 'fr'
        },
        allDayDefault: false,
        startParam: 'start',
        endParam: 'end',
        eventSources: [
            {
                url: "/Calendrier/GetEventsFormations",
                type: "POST",
                data: function () {
                    return {
                        id_educateur: $("#select_educateur").val()
                    };
                },
                dataType: "json",
                success: function (data) {
                },
                error: function (xhr, status, error) {
                    alert(error + " " + status);
                }
            }
        ],
        timeFormat: 'H:mm',
        eventClick: function (calEvent, jsEvent, view) {
            $("#intitule").empty();
            $("#debut").empty();
            $("#fin").empty();
            $.ajax({
                url: "/Calendrier/GetFormationById",
                type: "POST",
                data: { id_formation: calEvent.id },
                dataType: "json",
                success: function (data) {
                    if (data.ok) {
                        if (data.hasResult) {
                            $("#intitule_modif").val(data.titre);
                            $("#educateur_modif").val(data.educateur);
                            $("#educateur_modif").material_select();
                            $("#debut_modif").val(data.debut);
                            $("#fin_modif").val(data.fin);
                            Materialize.updateTextFields();
                            $("#modal_modif_formation").modal('open');
                            current_id = calEvent.id;
                        }
                    }
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            });
        },
        displayEventEnd: true
    });
    $("#form_modif_event").validate({
        errorPlacement: function (error, element) {
            error.insertBefore(element.parent("div"));
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            
            intitule_modif: {
                required: true
            },
            debut_modif: {
                required: true
            },
            fin_modif: {
                required: true
            }

        },
        messages: {

            intitule_modif: {
                required: "veuillez renseigner ce champs"
            },
            debut_modif: {
                required: "veuillez renseigner ce champs"
            },
            fin_modif: {
                required: "veuillez renseigner ce champs"
            }
        },
        submitHandler: function (form) {
            Update();
            return false;
        }
    });
    function Update() {
        var id_educateur = $("#educateur_modif").val();
        var title = $("#intitule_modif").val();
        var start = $("#debut_modif").val();
        var end = $("#fin_modif").val();
        $.ajax({
            url: "/Calendrier/UpdateFormation",
            type: "POST",
            data: { id: current_id, title: title, start: start, end: end, id_educateur : id_educateur },
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("la formation a été modifiée", 3000);
                    $("#modal_modif_formation").modal('close');
                    $("#calendrier_formation").fullCalendar('refetchEvents');
                }
                else {
                    Materialize.toast(data.error, 3000);
                }


            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
            }
        });
    }

    $("#btn_supprimer").click(function () {
        $.ajax({
            url: "/Calendrier/DeleteFormation",
            type: "POST",
            data: { id: current_id },
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("la formation a été supprimée", 3000);
                    $("#modal_modif_formation").modal('close');
                    $("#calendrier_formation").fullCalendar('refetchEvents');
                }
                else {
                    Materialize.toast(data.error, 3000);
                }


            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
            }
        });
    });

    $("#form_ajout_event").validate({
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {

            intitule: {
                required: true
            },
            debut: {
                required: true
            },
            fin: {
                required: true
            }

        },
        messages: {

            intitule: {
                required: "veuillez renseigner ce champs"
            },
            debut: {
                required: "veuillez renseigner ce champs"
            },
            fin: {
                required: "veuillez renseigner ce champs"
            }
        },
        submitHandler: function (form) {
            Create();
            return false;
        }
    });
    function Create() {
        var id_educateur = $("#educateur").val();
        var title = $("#intitule").val();
        var start = $("#debut").val();
        var end = $("#fin").val();
        $.ajax({
            url: "/Calendrier/SaveFormation",
            type: "POST",
            data: { title: title, start: start, end: end, id_educateur : id_educateur },
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("la formation a été créée", 3000);
                    $("#modal_ajout_formation").modal('close');
                    $("#calendrier_formation").fullCalendar('refetchEvents');
                }
                else {
                    Materialize.toast(data.error, 3000);
                }


            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
            }
        });
    }
    $("#add_modal").click(function () {
        $("#modal_ajout_formation").modal('open');
    });
})