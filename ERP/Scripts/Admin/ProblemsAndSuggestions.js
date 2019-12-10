function onloadTable() {

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
}
function ShowPhoto(table, id) {
    window.open('/Main/ShowPhoto?table=' + table + '&id=' + id);
}
function jsonDateFormat(jsonDate) {//json日期格式转换为正常格式
    try {
        var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        return date.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minutes + ":" + seconds;
    } catch (ex) {
        return "";
    }
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
            url: '/ProblemsAndSuggestions/GetList',           //请求后台的URL（*）
            method: 'get',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
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
            singleSelect: false,
            columns: [{
                checkbox: true
            }, {
                field: 'id',
                title: 'ID'
            }, {
                field: 'Problems',
                title: '问题', formatter: function (value, row, index) {
                    return '<a onclick="ShowMsg(\'' + value + '\')"  href="javascript:void(0)">' + value + '</a>';
                }
            }
            , {
                field: 'Suggestions',
                title: '建议', formatter: function (value, row, index) {
                    return '<a onclick="ShowMsg(\'' + value + '\')"  href="javascript:void(0)">' + value + '</a>';
                }
                
            }, {
                field: 'CreateTime',
                title: '提交时间', formatter: function (value, row, index) {
                    return jsonDateFormat(value);
                }
            }
            , {
                field: 'UsersId',
                title: '提交账号',
            }
            , {
                field: 'RealName',
                title: '真实姓名',
            }
              , {
                  field: 'phone',
                  title: '手机号',
              }
            , {
                field: 'Address',
                title: '联系地址',
            }
             , {
                 field: 'PhotoList',
                 title: '查看图片', formatter: function (value, row, index) {
                     return '<a onclick="ShowPhoto(\'ProblemsAndSuggestions\',' + row.id + ')"  href="javascript:void(0)" ><img style="height:40px;width:40px"  src="' + value + '" /><a/>';
                 }
             }
                , {
                    field: 'State',
                    title: '处理状态', formatter: function (value, row, index) {
                        if (value=="1") {
                            return "待处理"
                        }
                        if (value == "2") {
                            return "已处理"
                        }
                    }
                }
            ]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
            offset: (params.offset + 1),  //页码
            UsersId: $("#UsersId").val(),
            Stateinfo: $("#Stateinfo").find("option:selected").val()
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