$(document).ready(function () {
    //$.fullCalendar.formatDate("dd/MM/yyyy HH:mm:ss");
    $("#responsable_create").autocomplete({
        
        source: function (requete, reponse) { // les deux arguments représentent les données nécessaires au plugin
            $.ajax({
                type: "POST",
                url: "/Calendrier/AutocompleteLogin",
                data: '{username:"' + $("#responsable_create").val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    alert("ohoh !");
                    if (data.ok) {
                        reponse($.map(data.content, function (item) {
                            return {
                                label: item
                            };
                        }));
                    }
                    else {
                        reponse(["impossible de charger le contenu"]);
                    }
                }
            });
        },
        focus: function (event, ui) {   //Déclenché au survol
            $("#responsable_create").val(ui.item.label);
            $("#responsable_create").trigger("propertychange");//On informe le formulaire que le champs a été modifié
        },
        close: function (event, ui) {   //Déclenché lorsque le menu se ferme
            $("#responsable_create").trigger("propertychange");//On informe le formulaire que le champs a été modifié
        },
        open: function (event, ui) {    //Quand le menu apparaît
            $(".ui-autocomplete").css("z-index", 2000); //Important sinon le munu n'apparaît pas !
        }
    })._renderItem = function (ul, item) {  //hack pour mettre en évidence la selection
        // only change here was to replace .text() with .html()
        return $("<li></li>")
              .data("item.autocomplete", item)
              .append($("<a></a>").html(highlight(item.label, this.term)))
              .appendTo(ul);
    };
    var current_id;
    $("#calendrier_buvette").fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay',
            lang:'fr'
        },
        allDayDefault : false,
        startParam: 'start',
        endParam : 'end',
        eventSources: [
            {
                url: "/Calendrier/GetEventsBuvette",
                type: "POST",
                data: {},
                dataType : "json",
                success: function (data) {
                    
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            }
        ],
        timeFormat: 'H:mm',
        eventClick: function (calEvent, jsEvent, view) {
            $("#responsable").empty();
            $("#intitule").empty();
            $("#debut").empty();
            $("#fin").empty();
            $.ajax({
                url: "/Calendrier/GetEventBuvette",
                type: "POST",
                data: {id : calEvent.id},
                dataType: "json",
                success: function (data) {
                    if (data.ok) {
                        if (data.hasResult) {
                            $("#responsable").val(data.responsable);
                            $("#intitule").val(data.titre);
                            $("#debut").val(data.heure_debut);
                            $("#fin").val(data.heure_fin);
                            Materialize.updateTextFields();
                            $("#modal_modif_buvette").modal('open');
                            current_id = data.id;
                        }
                    }
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            });

        },
        displayEventEnd : true
    });

    $("#form_modif_event").submit(function () {
        var responsable = $("#responsable").val();
        var titre = $("#intitule").val();
        var debut = $("#debut").val();
        var fin = $("#fin").val();
        $.ajax({
            url: "/Calendrier/UpdateEventBuvette",
            type: "POST",
            data: { id: current_id, debut : debut, fin : fin, titre : titre, responsable: responsable },
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("L'évenement a été modifié", 3000);
                    $("#calendrier_buvette").fullCalendar('refetchEvents');
                    $("#modal_modif_buvette").modal('close');
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
        return false;
    });
    $("#create_buvette").validate({
        errorPlacement: function(error, element) {
            error.insertAfter(element.parent("div"));
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            responsable_create: {
                required : true
            },
            intitule_create: {
                required : true
            },
            debut_create: {
                required : true
            },
            fin_create: {
                required : true
            }

        },
        messages: {
            responsable_create: {
                required: "veuillez renseigner ce champs"
            },
            intitule_create: {
                required: "veuillez renseigner ce champs"
            },
            debut_create: {
                required: "veuillez renseigner ce champs"
            },
            fin_create: {
                required: "veuillez renseigner ce champs"
            }
        },
        submitHandler: function (form) {
            create();
            return false;
        }
    });

    function create() {
        var responsable = $("#responsable_create").val();
        var titre = $("#intitule_create").val();
        var debut = $("#debut_create").val();
        var fin = $("#fin_create").val();
        $.ajax({
            url: "/Calendrier/SaveEventBuvette",
            type: "POST",
            data: { debut: debut, fin: fin, titre: titre, responsable: responsable },
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("L'évenement a été créé", 3000);
                    $("#calendrier_buvette").fullCalendar('refetchEvents');
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
            url: "/Calendrier/DeleteEventBuvette",
            type: "POST",
            data: { id : current_id },
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("L'évenement a été supprimé", 3000);
                    $("#calendrier_buvette").fullCalendar('refetchEvents');
                    $("#modal_modif_buvette").modal('close');
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
})
