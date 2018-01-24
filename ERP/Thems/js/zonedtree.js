var d = new dTree('d');
$.getJSON("../../Ashx/Company.ashx", function (data) {
    $.each(data, function (k, v) {
        var ddwid = data[k].dwid;
        var dpid = data[k].pid;
        var ddwname = data[k].dwname;
        var bz = data[k].bz;
        d.add(ddwid, dpid, ddwname, "javascript:clickfun('" + ddwid + "','" + dpid + "','" + ddwname + "')");
    });
    document.getElementById("dtree").innerHTML = d;
});

function Zone()
{
    var d = new dTree('d');
    $.getJSON("../../Ashx/Company.ashx", function (data) {
        $.each(data, function (k, v) {
            var ddwid = data[k].dwid;
            var dpid = data[k].pid;
            var ddwname = data[k].dwname;
            var bz = data[k].bz;
            d.add(ddwid, dpid, ddwname, "javascript:clickfun('" + ddwid + "','" + dpid + "','" + ddwname + "')");
        });
        document.getElementById("dtree").innerHTML = d;
    });
}
