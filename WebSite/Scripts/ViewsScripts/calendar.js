function Calendar() {
    var jsonData;

    function loadCalendar() {
        $('#calendar').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            defaultView: 'agendaDay',
            navLinks: true,
            editable: false,
            minTime: "07:00:00",
            maxTime: "22:00:00",
            timeFormat: 'H:mm',
            events: {
                url: '/ScheduleData/ScheduleCalendar',
                borderColor: "#4682B4",
                success: function (events) {
                    jsonData = events;
                },
                error: function () {
                    alert("Calendar Error!");
                }
            },
            viewRender: function (view) {
                if (typeof (timelineInterval) != 'undefined') {
                    window.clearInterval(timelineInterval);
                }
                timelineInterval = window.setInterval(setTimeline, 30000);
                try {
                    setTimeline();
                } catch (err) { }
                postEvent();
            },
            eventAfterAllRender: function (view) {
                if (jsonData !== undefined && view.intervalUnit == "day")
                    DisplayInfo();
            }
        });
    }

    function postEvent() {
        $("#calendar").fullCalendar("refetchEvents");
    }

    function setTimeline(view) {
        var parentDiv = $('.fc-slats:visible').parent();
        var timeline = parentDiv.children(".timeline");
        if (timeline.length == 0) { //if timeline isn't there, add it
            timeline = $("<hr>").addClass("timeline").css("z-index", "99999");
            parentDiv.prepend(timeline);
        }

        var curTime = new Date();

        var curCalView = $("#calendar").fullCalendar('getView');
        if (curCalView.intervalStart < curTime && curCalView.intervalEnd > curTime) {
            timeline.show();
        } else {
            timeline.hide();
            return;
        }

        if (curCalView.name !== "month") {
            var calMinTimeInMinutes = strTimeToMinutes(curCalView.opt("minTime"));
            var calMaxTimeInMinutes = strTimeToMinutes(curCalView.opt("maxTime"));
            var curSeconds = ((((curTime.getHours() * 60) + curTime.getMinutes()) - calMinTimeInMinutes) * 60) + curTime.getSeconds();
            var percentOfDay = curSeconds / ((calMaxTimeInMinutes - calMinTimeInMinutes) * 60);

            var topLoc = Math.floor(parentDiv.height() * percentOfDay);
            var timeCol = $('.fc-time:visible');
            timeline.css({ top: topLoc + "px", left: ((timeCol.outerWidth(true)) - 20) + "px" });

            if (curCalView.name == "agendaWeek") { //week view, don't want the timeline to go the whole way across
                var dayCol = $(".fc-today:visible");
                var left = dayCol.position().left + 1;
                var width = dayCol.width() + 1;
                timeline.css({ left: left + "px", width: width + "px" });
            }
        }
    }

    function strTimeToMinutes(str_time) {
        var arr_time = str_time.split(":");
        var hour = parseInt(arr_time[0]);
        var minutes = parseInt(arr_time[1]);
        return ((hour * 60) + minutes);
    }

    function DisplayInfo() {
        $.each(jsonData, function (i, item) {
            var rows = "";
            var depStat = "";
            var reqStat = "";
            var succNum = 0;
            var unSuccNum = 0;
            var froStat = 0;
            var condition = "";
            //collecting data for displaying

            //var b = '<button  style="border-radius:20px;margin-left: 21px;margin-top: -8px;" class="glyphicon glyphicon-eye-open btn-sxs btn btn-info" type=button onclick=sheduler.changs(' + whl.ID + ')></button> '

            //var s = (function () {
            //    //prod i deploy
            //    if (item.actionId == 0 && item.deploy == 1) {
            //        return '<tr><td style="font-size:80%;">' + whl.ID + '</td><td style="font-size:80%;">' + whl.whl + '</td><td style="font-size:80%;text-align:center"> <a href="/WHLInfo/' + whl.ID + '" target="_blank"> <font size="1.8px">Details</font>  </a> </td><td style="font-size:80%;text-align:center">' + depStat + '</td><td>' + b + '</td></tr>';
            //    }
            //    //neprod i deplu
            //    if (item.actionId != 0 && item.deploy == 1) {
            //        return '<tr><td style="font-size:80%;">' + whl.ID + '</td><td style="font-size:80%;">' + whl.whl + '</td><td style="font-size:80%;text-align:center"> <a href="/WHLInfo/' + whl.ID + '" target="_blank"> <font size="1.8px">Details</font> </a> </td><td style="font-size:80%;text-align:center">' + depStat + '</td><td>' + '</td></tr>';
            //    }
            //    // pord i nedeploy
            //    if (item.deploy == 0 && item.actionId == 0) {
            //        return '<tr><td style="font-size:80%;">' + whl.ID + '</td><td style="font-size:80%;">' + whl.whl + '</td><td style="font-size:80%;text-align:center"> Details </td><td style="font-size:80%;text-align:center">' + depStat + '</td><td>' + b + '</td></tr>';
            //    }

            //    return '<tr><td style="font-size:80%;">' + whl.ID + '</td><td style="font-size:80%;">' + whl.whl + '</td><td style="font-size:80%;text-align:center"> Details </td><td style="font-size:80%;text-align:center">' + depStat + '</td><td>' + '</td></tr>';

            //}).call();

            //rows += s;

            $(".fc-content").each(function () {
                if ($(".fc-title", $(this)).text() == item.title) {
                    $(this)
                    .prepend(
                        $("<span>")
                            .append($('<span data-toggle="tooltip" data-placement="bottom" title="Details : ' + item.title + '">').addClass("glyphicon glyphicon-info-sign btn btn-sxs btn-info btnEdit"))
                            .css("margin-top", "2px").css("margin-right", "4px").css("float", "right")
                            .click(function () {
                                bootbox.dialog({
                                    title: "Discipline: " + item.title + "'",
                                    message:
                                        "<span><b>Type:</b></span><pre>" + item.Type + "</pre>" +
                                        //"<span><b>Information</b></span>" +                              TABLICATAAA
                                        //'<div style="max-height:200px;overflow:auto;">' +
                                        //'<table style="border-collapse:collapse;"class="table table-striped table-hover table-condensed">' + "<tr>" +
                                        //"<th>" + "ID" + "</th>" +
                                        //"<th>" + "WHL" + "</th>" +
                                        //"<th>" + "Link" + "</th>" +
                                        //"<th>" + "Status" + "</th>" +
                                        //"<th>" + "Compare" + "</th>" + "</tr>" +
                                        //rows +
                                        //"</table>" +
                                        //'</div>' +

                                        "<span><b>Topic:</b></span><pre>" + item.Topic + "</pre>" +
                                        "<span><b>Room:</b></span><pre>" + item.Room + "</pre>" +
                                        "<span><b>Teacher:</b></span><pre>" + item.TeacherName + "</pre>" +
                                        "<span><b>Description:</b></span><pre>" + item.FilePath + "</pre>" +
                                        "<span><b>Start Time:</b></span><pre>" + item.start + "</pre>" +
                                         "<span><b>End Time:</b></span><pre>" + item.end + "</pre>",

                                    className: "shedulerClass",

                                    buttons: {

                                        success: {
                                            label: "Teacher profile", className: "btn-success btn-edit", callback: function () { return dsUtil.onEditClick(item); }
                                        }
                                    }
                                });

                            })
                            )
                };
            });
        });
    }
    return {
        initCalendar: function () {
            loadCalendar();
        }
    }
}