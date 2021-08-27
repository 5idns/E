// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

//判断字符是否为空的方法
function isEmpty(obj) {
    if (typeof obj == "undefined" || obj == null || obj == "") {
        return true;
    } else {
        return false;
    }
}

function formatDateTime(obj) {
    if (isEmpty(obj)) {
        return "-";
    }
    //使用正则表达式将时间属性中的非数字（\D）删除
    //并把得到的毫秒数转换成数字类型
    var milliseconds = parseInt(obj.replace(/\D/igm, ""));
    //实例化一个新的日期格式，使用1970 年 1 月 1 日至今的毫秒数为参数
    var date = new Date(milliseconds);
    return date.toLocaleString();
}

function getTimeRangeStart(timeRange) {
    var time = timeRange.val();
    var times = time.split(" / ");
    var sTime = times[0];
    return sTime;
}

function getTimeRangeEnd(timeRange) {
    var time = timeRange.val();
    var times = time.split(" / ");
    var eTime = times[1];
    return eTime;
}

(function ($) {
    var DateRange = function (el, opt) {
        if (el) {
            this.$element = el;
            this.defaults = {
                'startTimeName': 'startTime',
                'endTimeName': 'endTime',
                'startDate': moment(),
                'endDate': moment()
            };
            this.options = $.extend({}, this.defaults, opt);

            this.startTimeObj =
                $('<input id="' +
                    this.options.startTimeName +
                    '" type="text" hidden="hidden" name="' +
                    this.options.startTimeName +
                    '" />');
            this.endTimeObj =
                $('<input id="' +
                    this.options.endTimeName +
                    '" type="text" hidden="hidden" name="' +
                    this.options.endTimeName +
                    '" />');
            this.$element.parent().append(this.startTimeObj);
            this.$element.parent().append(this.endTimeObj);
            this.init(this.$element); 
        }
    };
    DateRange.prototype = {
        //初始化函数
        init: function (el) {
            var self = el;
            var $this = this;
            if (self) {
                self.daterangepicker({
                        locale: {
                            "format": "YYYY-MM-DD", // 显示格式
                            "separator": " / ", // 两个日期之间的分割线
                            // 中文化
                            "applyLabel": "确定",
                            "cancelLabel": "取消",
                            "fromLabel": "开始",
                            "toLabel": "结束",
                            "daysOfWeek": ["日", "一", "二", "三", "四", "五", "六"],
                            "monthNames": ["一月", "二月", "三月", "四月", "五月", "六", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                            "firstDay": 1
                        },
                        "maxDate": new Date(),
                        "startDate": $this.options.startDate,
                        "endDate": $this.options.endDate
                    },
                    function(start, end, label) {
                        //console.log("daterangepicker", self.val(), start.toLocaleString(), end.toLocaleString());
                        try {
                            $this.setTimeWithDate(start, end);
                        } catch (e) {
                            console.log(e);
                        }

                    });
                $this.setTime();
            }
        },
        setTime: function () {
            var time = this.$element.val();
            if (time) {
                var times = time.split(" / ");
                this.startTimeObj.val(times[0]);
                this.endTimeObj.val(times[1]);
            }
        },
        setTimeWithDate: function (startTime, endTime) {
            this.startTimeObj.val(startTime.format("YYYY-MM-DD"));
            this.endTimeObj.val(endTime.format("YYYY-MM-DD"));

        },
        GetStartDate: function () {
            return this.startTimeObj.val();
        },
        GetEndDate: function () {
            return this.endTimeObj.val();
        }
    }

    //挂载到$的原型中，初始化实体类。
    $.fn.DateRange = function (options) { return new DateRange(this, options); }
})(jQuery);

(function ($) {
    var ModalAlert = function(el, opt) {
        var template = '<div class="modal modal-blur fade" id="modal-danger" tabindex="-1" role="dialog" aria-hidden="true">';
        template += '  <div class="modal-dialog modal-sm modal-dialog-centered" role="document">';
        template += '    <div class="modal-content">';
        template += '      <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>';
        template += '      <div class="modal-status bg-danger"></div>';
        template += '      <div class="modal-body text-center py-4">';
        template += '        <svg xmlns="http://www.w3.org/2000/svg" class="icon mb-2 text-danger icon-lg" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M12 9v2m0 4v.01" /><path d="M5 19h14a2 2 0 0 0 1.84 -2.75l-7.1 -12.25a2 2 0 0 0 -3.5 0l-7.1 12.25a2 2 0 0 0 1.75 2.75" /></svg>';
        template += '        <h3></h3>';
        template += '        <div class="text-muted"></div > ';
        template += '      </div>';
        template += '      <div class="modal-footer">';
        template += '        <div class="w-100">';
        template += '          <div class="row">';
        template += '            <div class="col"><a href="javascript:void(0);" class="btn btn-danger w-100" data-bs-dismiss="modal">';
        template += '              </a></div>';
        template += '          </div>';
        template += '        </div>';
        template += '      </div>';
        template += '    </div>';
        template += '  </div>';
        template += '</div>';

        this.RootObj = $("<div/>");
        $('body').append(this.RootObj);
        this.defaults = {
            'Title': '确认吗？',
            'Text': '你确认要执行此操作？',
            'ClassName': 'bg-danger',
            'Icon': '<svg xmlns="http://www.w3.org/2000/svg" class="icon mb-2 text-danger icon-lg" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M12 9v2m0 4v.01" /><path d="M5 19h14a2 2 0 0 0 1.84 -2.75l-7.1 -12.25a2 2 0 0 0 -3.5 0l-7.1 12.25a2 2 0 0 0 1.75 2.75" /></svg>',
            'SureText': '确认',
            'Template': template,
            'OnSure': function (control) { console.log('OnSure', control); }
        };
        this.$element = el;

        this.options = $.extend({}, this.defaults, opt);
    }
    ModalAlert.prototype = {
        //初始化函数
        init: function () {
        },
        ShowSuccess: function (opt) {
            this.defaults = {
                'Title': '成功',
                'Text': '操作成功！',
                'ClassName': 'bg-success',
                'Icon': '<svg xmlns="http://www.w3.org/2000/svg" class="icon mb-2 text-green icon-lg" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><circle cx="12" cy="12" r="9" /><path d="M9 12l2 2l4 -4" /></svg>',
                'SureText': '确认',
                'CancelText': '取消',
                'Template': this.options.Template,
                'OnSure': function (control) { console.log('OnSuccessSure', control); },
                'OnCancel': function (control) { console.log('OnSuccessCancel', control); },
            };
            var successOptions = $.extend({}, this.defaults, opt);
            var $this = this;
            var alertModalObj = $(successOptions.Template);
            alertModalObj.find(".modal-dialog>.modal-content>.modal-body>svg").remove();
            alertModalObj.find(".modal-dialog>.modal-content>.modal-body").prepend(successOptions.Icon);
            alertModalObj.find(".modal-dialog>.modal-content>.modal-body>h3").text(successOptions.Title);
            alertModalObj.find(".modal-dialog>.modal-content>.modal-body>div").text(successOptions.Text);
            alertModalObj.find(".modal-dialog>.modal-content>.modal-status").removeClass('bg-danger').addClass(successOptions.ClassName);
            alertModalObj.find(".modal-dialog>.modal-content>.modal-footer>.w-100>.row>.col>.btn-danger")
                .text(successOptions.SureText)
                .one('click', function () { successOptions.OnCancel($this); });
            $this.RootObj.append(alertModalObj);

            alertModalObj.on('hidden.bs.modal',
                function () {
                    alertModalObj.remove();
                });

            alertModalObj.modal('show');
        },
        ShowError: function (opt) {
            this.defaults = {
                'Title': '失败',
                'Text': '操作失败！',
                'ClassName': 'bg-warning',
                'Icon': '<svg xmlns="http://www.w3.org/2000/svg" class="icon mb-2 text-warning icon-lg" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"></path><circle cx="12" cy="12" r="9"></circle><path d="M10 10l4 4m0 -4l-4 4"></path></svg>',
                'SureText': '确认',
                'CancelText': '取消',
                'Template': this.options.Template,
                'OnSure': function (control) { console.log('OnErrorSure', control); },
                'OnCancel': function (control) { console.log('OnErrorCancel', control); },
            };
            var errorOptions = $.extend({}, this.defaults, opt);
            var $this = this;
            var alertModalObj = $(errorOptions.Template);
            alertModalObj.find(".modal-dialog>.modal-content>.modal-body>svg").remove();
            alertModalObj.find(".modal-dialog>.modal-content>.modal-body").prepend(errorOptions.Icon);
            alertModalObj.find(".modal-dialog>.modal-content>.modal-body>h3").text(errorOptions.Title);
            alertModalObj.find(".modal-dialog>.modal-content>.modal-body>div").text(errorOptions.Text);
            alertModalObj.find(".modal-dialog>.modal-content>.modal-status").removeClass('bg-danger').addClass(errorOptions.ClassName);
            alertModalObj.find(".modal-dialog>.modal-content>.modal-footer>.w-100>.row>.col>.btn-danger")
                .text(errorOptions.SureText)
                .one('click', function () { errorOptions.OnCancel($this); });
            $this.RootObj.append(alertModalObj);

            alertModalObj.on('hidden.bs.modal',
                function () {
                    alertModalObj.remove();
                });

            alertModalObj.modal('show');
        }
    }

    //挂载到$的原型中，初始化实体类。
    $.ModalAlert = function (options) { return new ModalAlert(this, options); }
})(jQuery);

(function ($) {
    var CustomizeConfirm = function (el, opt) {
        var template = '<div class="modal modal-blur fade" id="modal-danger" tabindex="-1" role="dialog" aria-hidden="true">';
        template += '  <div class="modal-dialog modal-sm modal-dialog-centered" role="document">';
        template += '    <div class="modal-content">';
        template += '      <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>';
        template += '      <div class="modal-status bg-danger"></div>';
        template += '      <div class="modal-body text-center py-4">';
        template += '        <svg xmlns="http://www.w3.org/2000/svg" class="icon mb-2 text-danger icon-lg" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M12 9v2m0 4v.01" /><path d="M5 19h14a2 2 0 0 0 1.84 -2.75l-7.1 -12.25a2 2 0 0 0 -3.5 0l-7.1 12.25a2 2 0 0 0 1.75 2.75" /></svg>';
        template += '        <h3></h3>';
        template += '        <div class="text-muted"></div > ';
        template += '      </div>';
        template += '      <div class="modal-footer">';
        template += '        <div class="w-100">';
        template += '          <div class="row">';
        template += '            <div class="col"><a href="javascript:void(0);" class="btn btn-white w-100" data-bs-dismiss="modal">';
        template += '              </a></div>';
        template += '            <div class="col"><a href="javascript:void(0);" class="btn btn-danger w-100" data-bs-dismiss="modal">';
        template += '              </a></div>';
        template += '          </div>';
        template += '        </div>';
        template += '      </div>';
        template += '    </div>';
        template += '  </div>';
        template += '</div>';

        this.RootObj = $("<div/>");
        $('body').append(this.RootObj);
        this.defaults = {
            'Title': '确认吗？',
            'Text': '你确认要执行此操作？',
            'ClassName': 'bg-danger',
            'Icon': '<svg xmlns="http://www.w3.org/2000/svg" class="icon mb-2 text-danger icon-lg" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M12 9v2m0 4v.01" /><path d="M5 19h14a2 2 0 0 0 1.84 -2.75l-7.1 -12.25a2 2 0 0 0 -3.5 0l-7.1 12.25a2 2 0 0 0 1.75 2.75" /></svg>',
            'SureText': '确认',
            'CancelText': '取消',
            'Template': template,
            'OnSure': function (control) { console.log('OnSure', control); },
            'OnCancel': function (control) { console.log('OnCancel', control); },
        };
        this.$element = el;

        this.options = $.extend({}, this.defaults, opt);
        this.init();
        //初始化函数 
    };
    CustomizeConfirm.prototype = {
        //初始化函数
        init: function () {
            var $this = this;
            var confirmObj = $($this.options.Template);
            confirmObj.find(".modal-dialog>.modal-content>.modal-body>svg").remove();
            confirmObj.find(".modal-dialog>.modal-content>.modal-body").prepend($this.options.Icon);
            confirmObj.find(".modal-dialog>.modal-content>.modal-body>h3").text($this.options.Title);
            confirmObj.find(".modal-dialog>.modal-content>.modal-body>div").text($this.options.Text);
            confirmObj.find(".modal-dialog>.modal-content>.modal-status").removeClass('bg-danger').addClass($this.options.ClassName);
            confirmObj.find(".modal-dialog>.modal-content>.modal-footer>.w-100>.row>.col>.btn-white")
                .text($this.options.CancelText)
                .one('click', function () { $this.options.OnCancel($this); });
            confirmObj.find(".modal-dialog>.modal-content>.modal-footer>.w-100>.row>.col>.btn-danger")
                .text($this.options.SureText)
                .one('click', function () { $this.options.OnSure($this); });
            $this.RootObj.append(confirmObj);

            confirmObj.on('hidden.bs.modal',
                function () {
                    confirmObj.remove();
                });

            confirmObj.modal('show');
        },
        ShowSuccess: function (opt) {
            this.defaults = {
                'Title': '成功',
                'Text': '操作成功！',
                'ClassName': 'bg-success',
                'Icon': '<svg xmlns="http://www.w3.org/2000/svg" class="icon mb-2 text-green icon-lg" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><circle cx="12" cy="12" r="9" /><path d="M9 12l2 2l4 -4" /></svg>',
                'SureText': '确认',
                'CancelText': '取消',
                'Template': this.options.Template,
                'OnSure': function (control) { console.log('OnSuccessSure', control); },
                'OnCancel': function (control) { console.log('OnSuccessCancel', control); },
            };
            var successOptions = $.extend({}, this.defaults, opt);
            var $this = this;
            var confirmObj = $(successOptions.Template);
            confirmObj.find(".modal-dialog>.modal-content>.modal-body>svg").remove();
            confirmObj.find(".modal-dialog>.modal-content>.modal-body").prepend(successOptions.Icon);
            confirmObj.find(".modal-dialog>.modal-content>.modal-body>h3").text(successOptions.Title);
            confirmObj.find(".modal-dialog>.modal-content>.modal-body>div").text(successOptions.Text);
            confirmObj.find(".modal-dialog>.modal-content>.modal-status").removeClass('bg-danger').addClass(successOptions.ClassName);
            confirmObj.find(".modal-dialog>.modal-content>.modal-footer>.w-100>.row>.col>.btn-white")
                .text(successOptions.CancelText)
                .one('click', function () { successOptions.OnCancel($this); });
            confirmObj.find(".modal-dialog>.modal-content>.modal-footer>.w-100>.row>.col>.btn-danger")
                .text(successOptions.SureText)
                .one('click', function () { successOptions.OnCancel($this); });
            $this.RootObj.append(confirmObj);

            confirmObj.on('hidden.bs.modal',
                function () {
                    confirmObj.remove();
                });

            confirmObj.modal('show');
        },
        ShowError: function (opt) {
            this.defaults = {
                'Title': '失败',
                'Text': '操作失败！',
                'ClassName': 'bg-warning',
                'Icon': '<svg xmlns="http://www.w3.org/2000/svg" class="icon mb-2 text-warning icon-lg" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"></path><circle cx="12" cy="12" r="9"></circle><path d="M10 10l4 4m0 -4l-4 4"></path></svg>',
                'SureText': '确认',
                'CancelText': '取消',
                'Template': this.options.Template,
                'OnSure': function (control) { console.log('OnErrorSure', control); },
                'OnCancel': function (control) { console.log('OnErrorCancel', control); },
            };
            var errorOptions = $.extend({}, this.defaults, opt);
            var $this = this;
            var confirmObj = $(errorOptions.Template);
            confirmObj.find(".modal-dialog>.modal-content>.modal-body>svg").remove();
            confirmObj.find(".modal-dialog>.modal-content>.modal-body").prepend(errorOptions.Icon);
            confirmObj.find(".modal-dialog>.modal-content>.modal-body>h3").text(errorOptions.Title);
            confirmObj.find(".modal-dialog>.modal-content>.modal-body>div").text(errorOptions.Text);
            confirmObj.find(".modal-dialog>.modal-content>.modal-status").removeClass('bg-danger').addClass(errorOptions.ClassName);
            confirmObj.find(".modal-dialog>.modal-content>.modal-footer>.w-100>.row>.col>.btn-white")
                .text(errorOptions.CancelText)
                .one('click', function () { errorOptions.OnCancel($this); });
            confirmObj.find(".modal-dialog>.modal-content>.modal-footer>.w-100>.row>.col>.btn-danger")
                .text(errorOptions.SureText)
                .one('click', function () { errorOptions.OnCancel($this); });
            $this.RootObj.append(confirmObj);

            confirmObj.on('hidden.bs.modal',
                function () {
                    confirmObj.remove();
                });

            confirmObj.modal('show');
        }
    }

    //挂载到$的原型中，初始化实体类。
    $.CustomizeConfirm = function (options) { return new CustomizeConfirm(this, options); }
})(jQuery);