var upload = function () {
    $('.filePicker').each(function (index, el) {
        var this_ = $(this);
        // 初始化Web Uploader
        var uploader = WebUploader.create({
            // 选完文件后，是否自动上传。
            auto: true,
            duplicate: true,//重复上传同一文件                   
            server: '/MFlower/UpServerPhoto', //上传接口                   
            pick: el,
            // 只允许选择图片文件。
            accept: {
                title: 'Images',
                extensions: 'gif,jpg,jpeg,bmp,png',
                mimeTypes: 'image/*'
            }
        });
        //传入表单参数
        uploader.on('uploadBeforeSend', function (obj, data) {
            var id = this_.attr('id');
            data = $.extend(data, {
                "id": id,
                "ArrangementId": $("#FlowerArrangementId").val()
            });
        });       
        //
        var af = $("#serveraf");
        if (af.length > 0) {
            uploader.on('fileQueued', function (file) {
                this_.children().remove();
                layer.msg("上传成功");
                var $img = $('<img >');
                this_.html($img);
                var thumbnailWidth = 80;
                var thumbnailHeight = 80;
                // 创建缩略图                    
                uploader.makeThumb(file, function (error, src) {
                    if (error) {
                        $img.replaceWith('<span>不能预览</span>');
                        return;
                    }
                    $img.attr('src', src);
                }, thumbnailWidth, thumbnailHeight);
            });
        }
        else {
            uploader.on('uploadSuccess', function (file, response) {
                if (response.msg === "1") {
                    layer.msg("上传成功");
                    this_.children().remove();
                    var $li = $('<img src=""  class="imgphoto" width="100" height="120" />');
                    $li.attr('src', response.path);
                    this_.html($li);
                }
                uploader.reset();
            });
        }
    });//end each  
}