function onloadTable() {

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
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

var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $("#tb_departments").bootstrapTable('destroy');//每次必须销毁表格 不然只能初始化一次       
        $('#tb_departments').bootstrapTable({
            url: '/FlowerActive/GetList',           //请求后台的URL（*）
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
                field: 'Id',
                title: 'ID'
            }, {
                field: 'FlowerWatchName',
                title: '对应花卉名称',
            }
             , {
                 field: 'Content',
                title: '活动内容',
             }, {
                 field: 'CreateTime',
                 title: '创建时间', formatter: function (value, row, index) {
                     return jsonDateFormat(value);
                 }
             }
            ]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
            offset: (params.offset),  //页码
            FlowerId:$("#FlowerId").val()
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
//
var layindex = 0;

//加载
function onloadDropdownlist() {
    var html;
    $.ajax({
        url: "/FlowerActive/GetFlowerList",
        type: "get",
        dataType: "json",
        success: function (data, state) {
            var companylist = data;
            for (var i = 0; i < companylist.length; i++) {
                html += '<option value="' + companylist[i].id + '">' + companylist[i].FlowerWatchName + '</option>';
            }
            $(".companyid").append(html);
        },
        error: function (m) { parent.layer.msg("error"); }

    });

}

//页面初始化
$(function () {
    onloadDropdownlist();
    onloadTable();
    $("#btn_add").click(function () {
        var layindex = parent.layer.open({
            type: 2,
            title: '增加',
            shadeClose: true,
            shade: 0.8,
            area: ['430px;', '320px;'],
            content: '/FlowerActive/Add',
            //end: function () { //层销毁后触发的回调
            //    onloadTable();
            //    parent.layer.close(layindex);
            //},
            btn: ['确定', '取消'], btnAlign: 'c',
            yes: function (index, layero) {
                ///返回主页的数据，为提交做准备
                if (layero.context.defaultView[1].CheckFrom()) {
                    //当点击‘确定’按钮的时候，获取弹出层返回的值
                    var editres = layero.context.defaultView[1].callbackdata();
                    $.ajax({
                        url: "/FlowerActive/Add",
                        type: "post",
                        data: editres,
                        success: function (data, state) {
                            $(".companyid").val("0");//重置查询条件
                            onloadTable();
                        },
                        error: function (msg) { alert(msg); },
                    });
                    //
                    parent.layer.close(index); //如果设定了yes回调，需进行手工关闭
                }
            },
            cancel: function () {
                layer.close(layindex);
            }
        });
    });
    $("#btn_edit").click(function () {
        var ids = $("#tb_departments").bootstrapTable('getSelections');
        //console.log(ids[0].Id);
        if (ids.length == 0) {
            parent.layer.msg('请先选中一行');
            return false;
        } else if (ids.length > 1) {
            parent.layer.msg('请选中一行,不可多行同时编辑');
            return false;
        }
        var layindex = parent.layer.open({
            type: 2,
            title: '修改',
            shadeClose: true,
            shade: 0.8,
            area: ['430px;', '300px;'],
            content: '/FlowerActive/Edit?id=' + ids[0].Id,            
            btn: ['确定', '取消'], btnAlign: 'c',
            yes: function (index, layero) {
                ///返回主页的数据，为提交做准备                        
                if (layero.context.defaultView[1].CheckFrom()) {
                    //当点击‘确定’按钮的时候，获取弹出层返回的值
                    var editres = layero.context.defaultView[1].callbackdata();
                    $.ajax({                       
                        url: "/FlowerActive/Edit",
                        type: "post",
                        data: editres,
                        success: function (data, state) {
                            $(".companyid").val("0");//重置查询条件
                            onloadTable();
                        },
                        error: function (msg) { alert(msg); },
                    });
                    //
                    parent.layer.close(index); //如果设定了yes回调，需进行手工关闭
                }
            },
            cancel: function () {
                layer.close(layindex);
            }
        });
    });
    $("#btn_delete").click(function () {
        var ids = $("#tb_departments").bootstrapTable('getSelections');
        if (ids.length == 0) {
            parent.layer.msg('请先选中一行');
            return false;
        } else if (ids.length > 1) {
            parent.layer.msg('请选中一行,不可多行删除');
            return false;
        }
        parent.layer.confirm('删除?', {
            btn: ['确定', '取消'] //按钮
        }, function () {
            $.ajax({
                url: '/FlowerActive/Delete?id=' + ids[0].Id,
                async: false,
                success: function (data) {
                    parent.layer.closeAll();
                    onloadTable();                 
                }
            })

        }, function () {
        });
    });
    //
})