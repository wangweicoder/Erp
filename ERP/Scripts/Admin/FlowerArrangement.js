function onloadTable() {

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
}

//(function (i, s, o, g, r, a, m) {
//    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
//        (i[r].q = i[r].q || []).push(arguments)
//    }, i[r].l = 1 * new Date(); a = s.createElement(o),
//m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
//})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
//ga('create', 'UA-36708951-1', 'wenzhixin.net.cn');
//ga('send', 'pageview');


var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $("#tb_departments").bootstrapTable('destroy');//每次必须销毁表格 不然只能初始化一次       
        $('#tb_departments').bootstrapTable({
            url: '/FlowerArrangement/GetList',           //请求后台的URL（*）
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
            singleSelect: false,                 //是否单选
            columns: [{
                checkbox: true
            }, {
                field: 'id',
                title: 'ID'
            }, {
                field: 'arrangement',
                title: '摆放位置'
            }
            , {
                field: 'Photo',
                title: '照片', formatter: function (value, row, index) {

                    if (value != "" && value != null) {
                        return '<img style="height:40px;width:40px"  src="' + value + '" />';
                    }

                }
               
            }, {
                field: 'Specifications',
                title: '规格(M)',
            }, {
                field: 'UnitPrice',
                title: '单价',
            }, {
                field: 'Count',
                title: '数量',
            }, {
                field: 'Total',
                title: '合计',
            }
            , {
                field: 'OwnedCompany',
                title: '所属公司',
            }
             , {
                 field: 'FlowerWatchName',
                 title: '对应花卉名称',
             }
              , {
                  field: 'FlowerWatchType',
                  title: '对应花卉品种',
              }, {
                  field: 'ImgORCodePath',
                  title: '二维码',
                  formatter: function (value, row, index) {
                      if (value == '' || value==null) {
                          return '';
                      }
                      return ' <a onclick="ShowPhotoInfo(\'' + value + '\')"  href="javascript:void(0)"><img style="height:40px;width:40px"  src="' + value + '" /></a>';
                  }
              }, {
                field: 'Remark',
                title: '备注',
                 formatter: function (value, row, index) {
                    if (value.length>50) {
                        return value.substring(0,49)+"……";
                    }
                    return value;

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
            arrangement: $("#arrangement").val(),
            belongUsersId: $("#belongUsersId").val()
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
function ShowPhotoInfo(values) {
    parent.layer.closeAll();
    $("#ShowPhotoImg").attr("src", values);
    var layindex = parent.layer.open({
        type: 1,
        shade: false,
        title: false, //不显示标题
        content: $('#PhotoShowDiv').html(), //捕获的元素，注意：最好该指定的元素要存放在body最外层，否则可能被其它的相对元素所影响
        cancel: function () {
            parent.layer.close(layindex);
        }
    });
    $("#ShowPhotoImg").attr("src", "");
}
//加载公司
function onloadDropdownlist() {
    var html;
    $.ajax({
        url: "/FlowerArrangement/GetCompanyList",
        type: "get",
        dataType: "json",
        success: function (data, state) {
            var companylist = data;
            for (var i = 0; i < companylist.length; i++) {
                html += '<option value="' + companylist[i].Value + '">' + companylist[i].Text + '</option>';
            }
            $(".companyid").append(html);
        },
        error: function (m) { parent.layer.msg("error"); }

    });

}
//导入Excel
function AlretAddByExcel() {
    parent.layer.open({
        type: 2,
        title: '导入Excel',
        shadeClose: true,
        shade: 0.8,
        area: ['300px;', '250px;'],
        content: '/FlowerArrangement/AddByExcel',
        end: function () {  //层彻底关闭后执行的回调
            onloadTable();
        },
        cancel: function () {
            parent.layer.close(layindex);
        }
    });
}
//导出Excel
function DownExcel() {
    var ids = "";
    var rows = $("#tb_departments").bootstrapTable('getSelections');
    for (var i = 0; i < rows.length; i++) {
        ids += rows[i].id + ",";
    }
    ids = ids.substring(0, ids.length - 1);
    var url;
    var arrangement = $("#arrangement").val();
    var belongUsersId = $("#belongUsersId").val();

    url = '/FlowerArrangement/DownloadExcel?ids=' + ids + '&limit=500&offset=1&arrangement=' + arrangement + '&belongUsersId=' + belongUsersId;

    location.href = url;

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
            area: ['630px;', '520px;'],
            content: '/FlowerArrangement/Add',
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
                        //dataType: "json"
                        url: "/FlowerArrangement/Add",
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
        //console.log(ids);
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
            area: ['630px;', '500px;'],
            content: '/FlowerArrangement/Edit?id=' + ids[0].id,
            //end: function (){ //层彻底关闭后执行的回调
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
                        //dataType: "json"
                        url: "/FlowerArrangement/Edit",
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
        }
        parent.layer.confirm('删除?', {
            btn: ['确定', '取消'] //按钮
        }, function () {
            var id = '';
            for (var i = 0; i < ids.length; i++) {
                id += ids[i].id + ",";
            }
            id = id.substr(0, id.length - 1);
            $.ajax({
                url: '/FlowerArrangement/Delete?ids=' +id,
                async: false,
                success: function (data) {
                    if (data == "1") {
                        onloadTable();
                    } else {
                        parent.layer.msg(data);
                    }                    
                    parent.layer.closeAll();
                }
            })

        }, function () {
        });
    });
    //导出二维码            
    $("#btn_ByOrcode").click(function () {
        var ids = "";
        var rows = $("#tb_departments").bootstrapTable('getSelections');
        for (var i = 0; i < rows.length; i++) {
            ids += rows[i].id + ",";
        }
        ids = ids.substring(0, ids.length - 1);
        var arrangement = $("#arrangement").val();
        var belongUsersId = $("#belongUsersId").val();
        //var url = '/FlowerArrangement/DownloadOrCode?ids=' + ids + '&limit=9000&offset=1&arrangement=' + arrangement + '&belongUsersId=' + belongUsersId;
        //location.href = url;
        $.ajax({
            url: '/FlowerArrangement/DownloadOrCode?ids=' + ids + '&limit=9000&offset=1&arrangement=' + arrangement + '&belongUsersId=' + belongUsersId,
            async: true,
            success: function (data) {
                if (data.code == "1") {
                    location.href = data.msg;
                }
                else {
                    parent.layer.open({
                        type: 0,
                        title: '提示',
                        shadeClose: true,
                        shade: 0.8,
                        //area: ['630px;', '500px;'],
                        content: '导出失败' + data.msg
                    });
                }
            },
            error: function (data) {

            }
        })
    });
})