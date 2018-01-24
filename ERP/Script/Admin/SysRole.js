function onloadTable() {

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
}

(function (i, s, o, g, r, a, m) {
    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
        (i[r].q = i[r].q || []).push(arguments)
    }, i[r].l = 1 * new Date(); a = s.createElement(o),
m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
ga('create', 'UA-36708951-1', 'wenzhixin.net.cn');
ga('send', 'pageview');


var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $("#tb_departments").bootstrapTable('destroy');//每次必须销毁表格 不然只能初始化一次
        $('#tb_departments').bootstrapTable({
            url: '/SysRole/Sys_Base_Role',           //请求后台的URL（*）
            method: 'get',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                    //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            //height: 500,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                checkbox: true
            }, {
                field: 'DeCode',
                title: $("#Bode").val()
            }, {
                field: 'Name',
                title: $("#BodeName").val()
            }
            , {
                field: 'AddUser',
                title: $("#AddUser").val()
            }
            , {
                title: $("#Operate").val(), field: '#', align: 'center', width: '25%',
                formatter: function (value, row, index) {
                    var e = '<button id="btn_edit" type="button" class="btn btn-default btn-xs" onclick="parent.layer.open({type: 2,title:\'' + $("#Edit").val() + '[<b> ' + row.Name + '</b> ]\',shadeClose: true,shade: 0.8,area: [\'380px\', \'60%\'],content:\'/SysRole/Edit?Code=' + row.DeCode + '\',end: function () {onloadTable();}});" style="margin-right:5px"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span> ' + $("#Edit").val() + '</button>'
                    var d = '<button id="btn_edit" type="button" class="btn btn-default btn-xs" onclick="parent.layer.open({type: 2,title:\'' + $("#UserRoleSetting").val() + '[<b> ' + row.Name + '</b> ]\',shadeClose: true,shade: 0.8,area: [\'90%\', \'85%\'],content:\'/SysRole/Role?Code=' + row.DeCode + '\'});" style="margin-right:5px"><span class="glyphicon glyphicon-cog" aria-hidden="true"></span> ' + $("#UserRoleSetting").val() + '</button>'
                    var f = '<button id="btn_edit" type="button" class="btn btn-default btn-xs" onclick="parent.layer.open({type: 2,title:\'' + $("#UserRoleSetting").val() + '[<b> ' + row.Name + '</b> ]\',shadeClose: true,shade: 0.8,area: [\'90%\', \'85%\'],content:\'/SysRole/RoleOperate?Code=' + row.DeCode + '\'});" style="margin-right:5px"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span> ' + $("#UserButtonSetting").val() + '</button>'
                    var g = '<button id="btn_view" type="button" onclick="parent.layer.alert(\'' + row.Name + '\')" class="btn btn-default btn-xs"><span class="glyphicon glyphicon-zoom-in" aria-hidden="true"></span> ' + $("#Select").val() + '</button>';
                    return e + d + f + g;
                }
            }]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,    //页面大小
            offset: params.offset,  //页码
            code: document.getElementById("code").value,
            name: document.getElementById("dename").value
        };
        return temp;
    };
    return oTableInit;
};


var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};

    oInit.Init = function () {
        //绑定@Html.Raw(Web.LanguageHelper.GetLangbyKey("Add")) 修改事件
    };

    return oInit;
};