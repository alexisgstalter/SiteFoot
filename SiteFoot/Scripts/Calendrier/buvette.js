$(document).ready(function () {
    //$.fullCalendar.formatDate("dd/MM/yyyy HH:mm:ss");
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

        }
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
})
