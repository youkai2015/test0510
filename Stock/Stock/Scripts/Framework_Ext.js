/*常量*/
var FF_MsgBoxButtons = {
    Btn_OK: 1,
    Btn_Yes: 2,
    Btn_No: 4,
    Btn_YesNo: 6,
    Btn_Cancel: 8,
    Btn_OkCancel: 9,
    Btn_YesNoCancel: 14
};

var FF_MsgBoxIcons = {
    Error: Ext.baseCSSPrefix + 'message-box-error',
    Question: Ext.baseCSSPrefix + 'message-box-question',
    Info: Ext.baseCSSPrefix + 'message-box-info',
    Warning: Ext.baseCSSPrefix + 'message-box-warning'
}

$.ajaxSetup({ cache: false });

Ext.override(Ext.form.DisplayField, {
    getValue: function () {
        return this.value;
    },
    setValue: function (v) {
        this.value = v;
        this.setRawValue(this.formatValue(v));
        return this;
    },
    formatValue: function (v) {
        var d = new Date(v);
        if (this.dateFormat && Ext.isDate(d)) {
            return Ext.Date.format(d, this.dateFormat);
        }
        if (this.numberFormat && typeof (Number(v)) == 'number') {
            return Ext.util.Format.number(Number(v), this.numberFormat);
        }
        return v;
    }
});

Ext.define('FFCOMBOMODEL', {
    extend: 'Ext.data.Model',
    idProperty: 'v',
    fields: [
        { type: 'string', name: 'k' },
        { type: 'string', name: 'v' }
    ]
});

Ext.define('Ext.ux.SearchGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.searchgrid',
    triggerBtnID: undefined
});

Ext.define('Ext.ux.CusColumn', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.cuscolumn',
    colType: 'normal'
});

function FF_GetCookie(key) {
    var rv = $.cookie(key);
    if (rv == null) {
        return "";
    }
    else {
        return rv;
    }
}

function FF_WithCheckBoxChange(self, newval) {
    var f = self.up('fieldcontainer').down('field');
    f.setDisabled(!newval);
    if (newval == true) {
        f.focus(false, true);
    }
}

function FF_WithCheckBoxBoxready(self, enabled) {
    self.setValue(enabled);
    self.up('fieldcontainer').down('field').setDisabled(!enabled);
}

function FF_MakeMLWindow(maintext, infos, minLength, maxLength, longName, fieldName, title, lanOK, lanCancel, ftype) {
    var tabs = Ext.createWidget('tabpanel', {
        activeTab: 0,
        deferredRender: false,
        layout: 'fit',
        defaults: {
            bodyPadding: 10
        },
        listeners: {
            afterrender: function (self) {
                var tab = self.getActiveTab();
                var input;
                input = tab.down('field');
                if (input != undefined) {
                    input.focus(false, true);
                }
                var knav = new Ext.util.KeyNav({
                    target: self.getEl(),
                    tab: function (e) {
                        var acttab = self.getActiveTab();
                        var count = self.items.getCount();
                        var ci = self.items.indexOf(acttab);
                        var newci = ci + 1;
                        if (newci == count) {
                            newci = 0;
                        }
                        self.setActiveTab(newci);
                    },
                    scope: self
                });
            },
            tabchange: function (self, tab) {
                var input;
                input = tab.down('field');
                if (input != undefined) {
                    window.setTimeout(
                        function () {
                            input.focus();
                        },
                        100
                    );
                }
            }
        }
    });
    var width = maintext.inputEl.getWidth();
    var height = maintext.inputEl.getHeight();
    $.each(infos, function (n, v) {
        var name = "dag" + longName + "[" + n + "]." + fieldName;
        var id = name.replace(/\./g, "_").replace(/\[/g, "_").replace(/\]/g, "_");
        tabs.add({
            title: v.LanguageName,
            layout: { type: 'vbox', align: 'stretch' },
            items: [{
                name: name,
                itemId: id,
                hideLabel: true,
                validateOnBlur: false,
                minLength: minLength,
                maxLength: maxLength,
                width: width,
                height: height,
                listeners: {
                    afterrender: function (a) {
                        var iid = id;
                        var hidden = maintext.up('form').query("#" + iid.substr(3))[0];
                        a.setRawValue(hidden.getRawValue());
                    }
                },
                xtype: ftype
            }]
        });
    });
    tabs.setActiveTab(0);
    var win = new Ext.Window({
        modal: 'true',
        layout: 'fit',
        title: title,
        buttons: [
        {
            xtype: 'button',
            type: 'submit',
            text: lanOK,
            listeners: {
                "click": function (self) {
                    var count = tabs.items.getCount();
                    var setmain = false;
                    for (i = 0; i < count; i++) {
                        var field = tabs.items.getAt(i).down('field');
                        var fieldvalue = field.getRawValue();
                        var cc = maintext.up('form').query("#" + field.getItemId().substr(3))[0]
                        cc.setRawValue(fieldvalue);
                        if (fieldvalue != "" && setmain == false) {
                            maintext.setRawValue(fieldvalue);
                            setmain = true;
                        }
                    }
                    win.close();
                    maintext.validate();
                    maintext.focus(false, true);
                    return false;
                }
            }
        },
        {
            xtype: 'button',
            text: lanCancel,
            listeners: {
                "click": function (self) {
                    win.close();
                    maintext.focus(false, true);
                    return false;
                }
            }
        }],
        listeners: {
            afterrender: function (wwin) {
                var btn = wwin.down('button[type=submit]');
                var knav = new Ext.util.KeyNav({
                    target: wwin.getEl(),
                    enter: function (e) {
                        if (btn != undefined) { btn.fireEvent('click', btn); }
                    },
                    scope: this
                });
            }
        },
        items: tabs
    });
    win.show();
}

function FF_SubmitFormOnEnter(a, b) {
    if (b.getKey() == b.ENTER) {
        var form = a.up('form');
        var btn = form.down('button[type=submit]');
        if (a.type == 'textarea') {
        }
        else {
            if (btn != undefined) { btn.fireEvent('click', btn); }
        }
    }
}

function FF_ExtGridColumnRender(value, metaData, rd, rowIndex, colIndex, st, view) {
    if (value == undefined) {
        return '';
    }
    //若存在checkbox则需要将colIndex减1
    if (view.grid.hascheckbox == true && view.grid.getSelectionModel() != null) {
        colIndex--;
    }
    var column = view.grid.columns[colIndex];
    var editor = column == null ? null : column.config.editor;
    if (editor != null) {
        if (editor.xtype == 'datefield' || editor.xtype == 'datetimefield') {
            var nv = Ext.util.Format.date(value, editor.format);
            rd.data[metaData.column.dataIndex] = nv;
            return nv;
        }
        if (editor.xtype == 'combobox') {
            var store = editor.store;
            var nv = "";
            $.each(store.data.items, function (n, v) {
                if (v.data.v == value) {
                    nv = v.data.k;
                    return;
                }
            })
            return nv;
        }
        if (editor.xtype == 'treecombo') {
            var items = editor.getStore().data.items;
            var nv = "";
            nv = FF_Recursion(items, value);
            return nv;
        }
        if (editor.xtype == 'uploadfield') {
            if (editor.uploadcontrol != null) {
                if (editor.delaydelete != null && editor.delaydelete != '') {
                    editor.deletepara.vmid = rd.data["ID"];
                    $.post(FF_GetCookie("VirtualDir") + "/WebApi/Home/DeleteAttachment", editor.deletepara, function (rv) {
                        rd.data["DONOTUSE_" + column.dataIndex] = '';
                    });
                }
                else {
                    for (var member in rd.data) {
                        if (rd.data[member] == value) {
                            rd.data["DONOTUSE_" + member] = editor.uploadcontrol.file_name;
                        }
                    }
                }
                return editor.uploadcontrol.file_name;
            }
        }
    }
    if (column != null && column.colType == 'kv') {
        return rd.data["DONOTUSE_" + column.dataIndex];
    }
    var patrn = /^<div style="(.*?)">(.*?)<\/div>$/;
    /*非字符串类型的就会抛错*/
    if (value == "`true`") {
        return "<div align='center'><input type='checkbox' checked='checked' disabled=1></div> ";
    }
    if (value == "`false`") {
        return "<div align='center'><input type='checkbox' disabled=1></div> ";
    }
    var r = value.match(patrn);
    if (r == null) {
        return value;
    }
    else {
        metaData.style = r[1];
        var rv = r[2];
        if (rv == '`true`') {
            return "<div align='center'><input type='checkbox' checked='checked' disabled=1></div> ";
        }
        else if (rv == '`false`') {
            return "<div align='center'><input type='checkbox' disabled=1></div> ";
        }
        else {
            return rv;
        }
    }
}

function FF_Recursion(nodes, value) {
    var nv = "";
    for (var i = 0; i < nodes.length; i++) {
        var v = nodes[i];
        if (v.data.id == value) {
            nv = v.data.text;
            return nv;
        } else if (v.childNodes.length > 0) {
            nv = FF_Recursion(v.childNodes, value);
            if (nv.length > 0) {
                return nv;
            }
        }
    }
    return nv;
}


Ext.form.Basic.prototype.findInvalid = function () {
    var me = this,
        invalid;
    Ext.suspendLayouts();
    invalid = me.getFields().filterBy(function (field) {
        var preventMark = field.preventMark, isValid;
        field.preventMark = true;
        isValid = field.validate();
        field.preventMark = preventMark;
        return !isValid;
    });
    Ext.resumeLayouts(true);
    return invalid;
};

Ext.form.Panel.prototype.firstinvalidfound = false;
Ext.override(Ext.Component, {
    ensureVisible: function (stopAt) {
        var p;
        this.ownerCt.bubble(function (c) {
            if (p = c.ownerCt) {
                if (p instanceof Ext.TabPanel) {
                    p.setActiveTab(c);
                } else if (p.layout.setActiveItem) {
                    p.layout.setActiveItem(c);
                }
            }
            return (c !== stopAt);
        });
        /*this.el.scrollIntoView(this.el.up(':scrollable'));*/
        return this;
    }
});

function FF_SetError(cmp, error) {
    cmp.setActiveErrors(error);
    var form = cmp.up('form');
    if (form != null && form.firstinvalidfound == false) {
        cmp.ensureVisible();
        cmp.focus(false, true);
        form.firstinvalidfound = true;
    }
}

function FF_LinkedChange(combo, linkedID, linkedUrl) {
    var linkedcombo = combo.up('form').query("[name=" + linkedID + "]")[0];
    if (linkedcombo == null || linkedcombo == undefined) { return false; }
    linkedcombo.setDisabled(true);
    if (linkedcombo.xtype != "itemselector" || linkedcombo.isClearValue == true) {
        linkedcombo.clearValue();
    }
    var id = "";
    var value = combo.getValue();
    Ext.Array.each(value, function (record) {
        id += "id=" + record + "&";
    });
    id += "1=1";
    linkedUrl = FF_GetCookie("VirtualDir") + linkedUrl + "?" + id;
    $.get(linkedUrl, "", function (data) {
        var linkedStore,
            ms = linkedcombo.initialConfig["multiSelect"],
            empty = linkedcombo.initialConfig["emptyText"];

        if (linkedcombo.xtype == "itemselector") {
            linkedStore = linkedcombo.getFromField().getStore();
            linkedStore.removeAll();

            var toItems = linkedcombo.getToField().getStore().data.items;
            if (ms != true && empty != null && empty != undefined && empty != "") {
                linkedStore.add({ 'k': empty, 'v': 'null' });
            }
            $.each(data, function (n2, v2) {
                var i;
                for (i = 0; i < toItems.length; i++) {
                    if (toItems[i].data.v == v2.Value) {
                        break;
                    }
                }
                if (i >= toItems.length) {
                    linkedStore.add({ 'k': v2.Text, 'v': v2.Value });
                }

            });
        } else if (linkedcombo.xtype == "treecombo") {
            if (ms != true && empty != null && empty != undefined && empty != "") {
                data = data.insertAt(0, { id: 'null', text: empty, leaf: true });
            }
            linkedcombo.setStore(new Ext.data.TreeStore({
                folderSort: false,
                root: {
                    text: 'Root',
                    id: null,
                    children: data
                }
            }));
        } else {
            linkedStore = linkedcombo.store;
            linkedStore.removeAll();
            if (ms != true && empty != null && empty != undefined && empty != "") {
                linkedStore.add({ 'k': empty, 'v': 'null' });
            }
            $.each(data, function (n2, v2) {
                linkedStore.add({ 'k': v2.Text, 'v': v2.Value });
            });
        }


        if (ms != true && linkedcombo.xtype == 'combobox') {
            var rec = linkedStore.getAt(0);
            if (rec != null) {
                linkedcombo.setValue(rec.get('v'));
            }
        }
        linkedcombo.fireEventArgs("select", [linkedcombo]);
        linkedcombo.setDisabled(false);
    });
}

function FF_SubmitForm(formid, buttonid, url, targetDivid) {
    var formobj = Ext.getCmp(formid);
    var buttonobj = Ext.getCmp(buttonid);
    if (formobj.isValid() == false) {
        var invalids = formobj.getForm().findInvalid().getAt(0);
        invalids.ensureVisible();
        invalids.focus(false, true);
        return;
    }
    FF_ShowMask(null, formobj);
    if (buttonobj != null) {
        buttonobj.disable();
    }

    var detailGridArray = formobj.query('grid');
    if (detailGridArray.length > 0) {
        for (var i = 0; i < detailGridArray.length; i++) {
            var initCfg = detailGridArray[i].initialConfig;
            if (initCfg.gridType == 'detailgrid') {
                var g = Ext.getCmp(detailGridArray[i].id);
                var sm = g.getSelectionModel();
                var items = new Array();
                if (sm != null && g.hascheckbox == true) {
                    items = sm.getSelection();
                } else {
                    items = g.getStore().data.items;
                }

                if (items.length == 0) {
                    formobj.add(Ext.create('Ext.form.field.Hidden', {
                        name: initCfg.dbsetname + '.DONOTUSECLEAR',
                        value: ''
                    }));
                }
                for (var j = 0; j < items.length; j++) {
                    var record = items[j].data;
                    for (var item in record) {
                        if (item.lastIndexOf("DONOTUSE") == 0) {
                            continue;
                        }
                        var vv = record[item];
                        formobj.add(Ext.create('Ext.form.field.Hidden', {
                            name: initCfg.dbsetname + '[' + j + '].' + item,
                            value: vv
                        }));
                    }
                }
            }
        }
    }

    var vals = formobj.getValues();

    $.each(vals, function (n, val) {
        if (val instanceof Array) {
            $.each(val, function (i, v) {
                if (v == "null") {
                    val[i] = "";
                }
            });
        } else if (val == "null") {
            vals[n] = "";
        };
    })
    $.post(url, vals, function (data, sta) {
        try {
            FF_CloseMask(formobj);
            buttonobj.enable();
        }
        catch (e) { }
        if (sta == 'success') {
            /*if (targetDivid == null || targetDivid == undefined) {*/
            $('body').append(data);
            /*}
            else {
                var div = $('#' + targetDivid + "ext");
                div.html(" ");
                div.html(data);
            }*/
        }
    });
}

function FF_LoadPage(Url, msg, token, domainid, entryurl) {
    var domainurl = "";
    Url = decodeURIComponent(Url);
    if (Url == "/") {
        Url = "/Home/FrontPage";
    }
    var Url2 = "";
    var popupurl = "";
    var visitUrl = "";
    var useIframe = false;
    if (Url.toLowerCase().indexOf("http://") < 0 && (domainid == null || domainid == undefined || domainid == "")) {
        Url2 = FF_GetCookie("VirtualDir") + Url;
        popupurl = FF_GetCookie("VirtualDir") + "/Home/PopUpIndex/#" + Url;
    }
    else {
        useIframe = true;
        if (domainid != null && domainid != undefined && domainid != "") {
            var u = jQuery.url.setUrl(Url);
            var addr = "";
            addr = u.attr("protocol") + "://" + u.attr("host");
            if (u.attr("port") != null && u.attr("port") != "") {
                addr += ":" + u.attr("port");
            }
            visitUrl = u.attr("path");
            if (u.attr("query") != null && u.attr("query") != "") {
                visitUrl += "?" + u.attr("query");
            }
            domainurl = visitUrl;
            var info = token.split("`");
            if (entryurl == undefined || entryurl == "") {
                entryurl = "/Home/PopUpIndex/#/WebApi/Login/Visit";
            }
            Url2 = addr + entryurl + "?itcode=" + info[0] + "&hash=" + info[1] + "&token=" + info[2] + "&path=" + encodeURIComponent(visitUrl) + "&domainid=" + domainid;
            popupurl = Url2;
        }
    }
    if (DONOTUSE_ISCTRL == true) {
        DONOTUSE_ISCTRL = false;
        window.open(popupurl, "_blank");
    }
    else {
        FF_ShowMask();
        DONOTUSE_CURRENTDOMAINID = domainid;
        if (useIframe == true) {
            FF_ClearMainPanel();
            var cp = Ext.getCmp("DONOTUSE_MAINPANEL");
            cp.add(Ext.create('Ext.ux.SimpleIFrame', {
                border: false,
                src: Url2
            }));
            FF_CloseMask();
            if (msg != undefined) {
                alert(msg);
            }
            if (domainid != null && domainid != undefined && domainid != "") {
                FF_SetHistory(domainurl + "::" + domainid, visitUrl, domainid);
            }
            else {
                FF_SetHistory(Url, null, domainid);
            }
        }
        else {
            $.get(Url2, function (data) {
                $('body').append(data);
                FF_CloseMask();
                if (msg != undefined) {
                    alert(msg);
                }
                FF_CloseMask();
                FF_SetHistory(Url, null, domainid);
            });
        }
    }
}

function FF_SetHistory(Url, menuUrl, did) {
    if (Url.indexOf("/WebApi/Home/Debug") != 0) {
        if ($.cookie(DONOTUSE_COOKIEPRE + DONOTUSE_WINDOWGUID + "FFLastPage") != null) {
            $.cookie(DONOTUSE_COOKIEPRE + DONOTUSE_WINDOWGUID + "FFLastPage2", $.cookie(DONOTUSE_COOKIEPRE + DONOTUSE_WINDOWGUID + "FFLastPage"));
        }
        else {
            $.cookie(DONOTUSE_COOKIEPRE + DONOTUSE_WINDOWGUID + "FFLastPage2", Url);
        }
        $.cookie(DONOTUSE_COOKIEPRE + DONOTUSE_WINDOWGUID + "FFLastPage", Url);
    }
    if (menuUrl == undefined || menuUrl == null) {
        FF_SetActiveMenu(Url, did);
    }
    else {
        FF_SetActiveMenu(menuUrl, did);
    }

    newToken = $.cookie(DONOTUSE_COOKIEPRE + DONOTUSE_WINDOWGUID + "FFLastPage");
    if (newToken == "/Home/FrontPage") {
        newToken = "/";
    }
    window.location.hash = Url;
}

function FF_OpenDialog(Url, Title, Width, Height, param, method, DialogID, CurrentWindowID, resizable) {
    if (IsNull(method)) {
        method = 'get';
    }
    Url = decodeURIComponent(Url);
    var url2 = FF_GetCookie("VirtualDir") + Url;
    if (url2.indexOf("?") < 0) {
        url2 = url2 + "?1=1";
    }
    var theForm;
    if (DONOTUSE_ISCTRL == true) {
        DONOTUSE_ISCTRL = false;
        window.open(FF_GetCookie("VirtualDir") + "/Home/PopUpIndex/#" + Url, "_blank");
    }
    else {
        var did = (DialogID == undefined || DialogID.length < 1) ? Ext.id() : DialogID;
        var listenresize = false;
        url2 += "&DONOTUSE_WindowID=" + CurrentWindowID + "," + did;
        var win = new Ext.Window({
            id: did,
            width: Width == null ? undefined : Width,
            height: Height == null ? undefined : Height,
            shadow: false,
            layout: 'fit',
            title: Title,
            modal: true,
            maximizable: true,
            autoScroll: false,
            loader: {
                ajaxOptions: { method: method },
                params: param,
                url: url2,
                renderer: 'html',
                scripts: true,
                autoLoad: true,
                callback: function (self) {
                    win.on({
                        afterlayout: {
                            fn: function (self) {
                                self.center();
                            },
                            scope: this,
                            single: true
                        },
                        resize: {
                            fn: function (self) {
                                self.center();
                            },
                            scope: this,
                            single: true
                        }
                    });
                }
            }
        });
        if (typeof (resizable) == 'boolean') {
            win.resizable = resizable;
        }
        win.showAt(-5000, -5000);
    }
}

function FF_GetURL(url) {
    $.get(FF_GetCookie("VirtualDir") + url, function (data) {
        $('#DONOTUSE_HIDDENPOSTDIV').html(data);
    })
}

function FF_PostURL(url, data) {
    $.post(FF_GetCookie("VirtualDir") + url, data, function (data) {
        $('#DONOTUSE_HIDDENPOSTDIV').html(data);
    })
}

function FF_CloseDialog(elementid) {
    try {
        var dia = Ext.getCmp(elementid);
        if (dia == undefined || dia.length == 0) {
            FF_ClearMainPanel();
        }
        else {
            dia.close();
        }
    }
    catch (e) { }
}

function FF_ClearMainPanel() {
    var cp = Ext.getCmp("DONOTUSE_MAINPANEL");
    var removed = cp.removeAll(true);
    $.each(removed, function (n, v) {
        var id = v.getId();
        try {
            var divid = id.slice(0, -3);
            $('#' + divid).remove();
        }
        catch (ex) { }
    })
}

function FF_CloseAllDialog() {
    $(".x-window").each(function (n, v) {
        var id = $(v).attr("id");
        if (id != null && id != "") {
            Ext.getCmp(id).close();
        }
    });
}

function FF_InitView(divName, windowid) {
    wid = windowid.split(",").pop();
    var ele = Ext.getCmp(wid);
    if (ele == undefined || ele == null || ele.length == 0) {
        ele = Ext.getCmp("DONOTUSE_MAINPANEL");
    }
    if (ele != null) {
        var removed = ele.removeAll(true);
        $.each(removed, function (n, v) {
            var id = v.getId();
            try {
                var divid = id.slice(0, -3);
                $('#' + divid).remove();
            }
            catch (ex) { }
        })
        var panel = Ext.create('Ext.container.Container', {
            id: divName + "ext",
            layout: {
                type: 'fit'
            }
        });
        ele.add(panel);
    }
}

function FF_Search(gridID, ispostback, isCollaspedAfterSearch, issaveincookie, posturl) {
    var grid = Ext.getCmp(gridID);
    if (grid == null) {
        return;
    }
    var selfbtn = Ext.getCmp(grid.triggerBtnID);
    if (selfbtn != null) {
        selfbtn.disable();
        var store = grid.store;
        var proxy = store.proxy;
        var form = selfbtn.up('form');
        if (form.hasInvalidField() == true) {
            selfbtn.enable();
            form.isValid();
            return;
        }
        else {
            form.isValid();
        }
        if (isCollaspedAfterSearch) {
            form.toggleCollapse();
        }
    }
    var vals = form.getValues();

    $.each(vals, function (n, v) {
        if (v == "null") {
            vals[n] = "";
        };
    })
    var pagecombo = null;
    if (selfbtn != null) {
        var combos = selfbtn.up().query("combobox");
        if (combos != null && combos.length > 0) {
            pagecombo = combos[combos.length - 1];
        }
    }
    var ps = -1;
    if (pagecombo != undefined) {
        ps = pagecombo.getValue();
    }
    vals["RecordsPerPage"] = ps;
    if (ispostback == true && posturl != undefined && grid.fired == false) {
        Ext.Ajax.request({
            url: posturl,
            method: 'POST',
            params: vals,
            callback: function (options, success, response) {
                if (success) {
                    $('body').append(response.responseText);
                }
                else {
                    if (selfbtn != null) {
                        selfbtn.enable();
                    }
                }
            }
        });
    }
    else {
        if (store.$className == "Ext.data.TreeStore") {
            proxy.setExtraParam("Searcher.ParentID", null);
            Ext.Object.each(vals, function (n, v) {
                proxy.setExtraParam(n, v);
            });
            /*proxy.setExtraParam("UniqueID", vmid);*/
            proxy.setExtraParam("SaveInCookie", issaveincookie);
            store.setRootNode({
                expanded: true
            });
        }
        else {
            if (proxy.url == undefined || proxy.url == null || proxy.url == "") {
                return;
            }
            store.setPageSize(ps);
            Ext.Object.each(vals, function (n, v) {
                proxy.setExtraParam(n, v);
            });
            /*proxy.setExtraParam("UniqueID", vmid);*/
            proxy.setExtraParam("SaveInCookie", issaveincookie);
            proxy.setExtraParam("RecordsPerPage", ps);
            store.load();
        }
    }
}

function FF_GridLoad(store, records, gridid) {
    var grid = Ext.getCmp(gridid);
    if (grid.el != undefined) {
        var checkboxHeader = $(grid.el.dom).find(".x-column-header-checkbox");
        checkboxHeader.removeClass("x-grid-hd-checker-on");
        var sm = grid.getSelectionModel();
        sm.deselectAll();
        var found = false;
        Ext.Array.each(records, function (item) {
            if (item.data.TempIsSelected == "1") {
                sm.select(item, true);
                found = true;
            }
        });
        if (found == true) {
            grid.getView().focusRow(0);
        }
    }
    setTimeout(function () {
        var btn = Ext.getCmp(grid.triggerBtnID);
        if (btn != undefined && btn != null) {
            btn.enable(true);
            $("#" + grid.triggerBtnID).removeClass("x-btn-default-toolbar-small-focus");
        }
    }, 100);
}

function FF_TreeGridStoreBeforeLoad(store, operation) {
    var id = operation.config.node.data.ID;
    store.proxy.setExtraParam("Searcher.ParentID", id);
}

function FF_GridBeforeRender(grid, vmid, autoload, expandAll) {
    var btn = Ext.getCmp(grid.triggerBtnID);
    var ispostback = IsNull(btn) || IsNull(btn.up('form')) || IsNull(btn.up('form').isPostBack) ? false : btn.up('form').isPostBack;
    if (autoload || ispostback) {
        if (IsNull(btn)) {
            grid.store.proxy.setExtraParam("UniqueID", vmid);
            grid.store.setRootNode({
                expanded: true
            });
        }
        else {
            grid.fired = true;
            btn.fireEventArgs("click", [btn, null, true]);
            grid.fired = false;
        }
    }
    if (expandAll == true) {
        grid.expandAll();
    }
}

function FF_TreeGridStoreLoad(store, records, gridid) {
    var grid = Ext.getCmp(gridid);
    if (grid.el != undefined) {
        var checkboxHeader = $(grid.el.dom).find(".x-column-header-checkbox");
        checkboxHeader.removeClass("x-grid-hd-checker-on");
        var sm = grid.getSelectionModel();
        /*sm.deselectAll();*/
        //sm.mode = "MULTI";
        var found = false;
        Ext.Array.each(records, function (item) {
            //sm.mode = "MULTI";
            //if (item.data.TempIsSelected == "1") {
            //    sm.select(item, true, false);
            //}
            //if (item.childNodes.length > 0) {
            //    Ext.Array.each(item.childNodes, function (son) {
            //        if (son.data.TempIsSelected == "1") {
            //            sm.select(son, true, false);
            //        }
            //    });
            //}
            //sm.deselect(item, true, false);
            //found = true;
            if (item.data.TempIsSelected == "1") {
                sm.select(item, true);
                //sm.select(item.childNodes, true);
                found = true;
            }
            //sm.mode = "SIMPLE";
        });
        if (found == true) {
            grid.getView().focusRow(0);
        }
    }
    setTimeout(function () {
        var btn = Ext.getCmp(grid.triggerBtnID);
        if (btn != undefined && btn != null) {
            btn.enable(true);
            $("#" + grid.triggerBtnID).removeClass("x-btn-default-toolbar-small-focus");
        }
    }, 100);
}

function FF_GridGetSelectedIDs(gridid) {
    var grid = Ext.getCmp(gridid);
    var sels = grid.getSelectionModel().getSelection();
    var rv = new Array();
    var regex = /(<([^>]+)>)/ig;
    $.each(sels, function (n, v) {
        rv.push(v.data.ID.replace(regex, ""));
    });
    return rv;
}

function FF_ResetSearchForm(formid) {
    Ext.Object.each(Ext.getCmp(formid).getForm().getFields().items, function (n, f) {
        if (f.getName().indexOf("Searcher.") == 0) {
            f.setValue(null);
        }
    });
}

function FF_GridReady(self, autoSearch) {
    var btn = Ext.getCmp(self.triggerBtnID);
    var ispostback = IsNull(btn) || btn.up('form') == null || btn.up('form').isPostBack == null ? false : btn.up('form').isPostBack;
    if (autoSearch || ispostback) {
        //if (IsNull(btn) || ispostback) {
        if (IsNull(btn)) {
            self.store.load();
        }
        else {
            self.fired = true;
            btn.fireEventArgs("click", [btn, null, true]);
            self.fired = false;
        }
    }
}

function FF_TreeGridReady(grid, autoSearch) {
    var tab = grid.up('tabpanel');
    if (tab != null) {
        var btn = Ext.getCmp(grid.triggerBtnID);
        var ispostback = btn.up('form') == null || btn.up('form').isPostBack == null ? false : btn.up('form').isPostBack;
        if (autoSearch || ispostback) {
            //if (IsNull(btn) || ispostback) {
            if (IsNull(btn)) {
                grid.store.setRootNode({ expanded: true });
            }
            else {
                self.fired = true;
                btn.fireEventArgs('click', [btn, null, true]);
                self.fired = false;
            }
        }
    }
}

function FF_AddParentID(windowid, viewdivid) {
    var view = Ext.getCmp(viewdivid + "ext");
    if (IsNull(view) == false) {
        var forms = view.query('form');
        if (forms != null) {
            Ext.Array.each(forms, function (obj, index) {
                obj.add({ xtype: 'hiddenfield', name: 'DONOTUSE_WindowID', itemId: 'DONOTUSE_WindowID', value: windowid });
            });
        }
    }
}

function FF_AddConnectionInfo(cs,dc, viewdivid) {
    var view = Ext.getCmp(viewdivid + "ext");
    if (IsNull(view) == false) {
        var forms = view.query('form');
        if (forms != null) {
            Ext.Array.each(forms, function (obj, index) {
                obj.add({ xtype: 'hiddenfield', name: 'CurrentCS', itemId: 'CurrentCS', value: cs });
                obj.add({ xtype: 'hiddenfield', name: 'CurrentDC', itemId: 'CurrentDC', value: dc });
            });
        }
    }
}

function FF_RefreshGrid(dialogid, index) {
    var panel = Ext.getCmp(dialogid);
    var grid = panel.query("searchgrid,treepanel")[index];
    if (grid != null) {
        var btn = Ext.getCmp(grid.triggerBtnID);
        if (btn == null) {
            grid.store.load();
        }
        else {
            btn.fireEvent("click", btn);
        }
    }
}

function FF_RefreshGridRow(dialogid, index, vmfullname, id) {
    var panel = Ext.getCmp(dialogid);
    var grid = panel.query("searchgrid,treepanel")[index];
    if (grid != null) {
        var sel = grid.getStore().getById(id);
        if (sel != null) {
            $.post(FF_GetCookie("VirtualDir") + "/WebApi/Home/GetListDataByID", { ID: id, FFVmName: vmfullname }, function (rv) {
                rv = rv.replace(/\\'/g, "'");
                var data = JSON.parse(rv);
                for (var member in sel.data) {
                    sel.set(member, data[member]);
                }
            });
        }
    }
}

function FF_RefreshGridByID(gridid) {
    var grid = Ext.getCmp(gridid);
    if (grid != null) {
        var btn = Ext.getCmp(grid.triggerBtnID);
        if (btn == null) {
            grid.store.load();
        }
        else {
            btn.fireEvent("click", btn);
        }
    }
}

function FF_CommonConditionSelect(combo, formid) {
    var value = combo.getValue();
    if (IsNull(value)) {
        FF_ResetSearchForm(formid);
    }
    else {
        $.get(FF_GetCookie("VirtualDir") + '/WebApi/Home/GetCondition/' + value, function (data, sta) {
            var jdata = JSON.parse(data);
            Ext.Object.each(Ext.getCmp(formid).getForm().getFields().items, function (n, f) {
                $.each(jdata, function (i, v2) {
                    if (v2.name == f.getName()) {
                        f.setValue(v2.value);
                    }
                });
            });
        });
    }
}

function FF_CommonConditionAdd(vmfullname, cccomboid, ccname, fn, errorfn) {
    var data = "ccname=" + ccname + "&vmfullname=" + vmfullname + "&";
    var cc = Ext.getCmp(cccomboid);
    var innerform = cc.up('form');
    Ext.Object.each(innerform.getValues(), function (n, v) {
        data += "ConditionName=" + n + "&";
        data += "ConditionValue=" + v + "&";
    });
    data += "1=1";
    $.post(FF_GetCookie("VirtualDir") + '/WebApi/Home/AddCondition', data, function (data) {
        if (data.rv == '1') {
            cc.store.add({ "k": ccname, "v": data.data });
            cc.setValue(data.data, true);
            fn();
        }
        else {
            errorfn(data.data);
        }
    })
}

function FF_CommonConditionSave(cccomboid, fn) {
    var cc = Ext.getCmp(cccomboid);
    var innerform = cc.up('form');
    var combovalue = cc.getValue();
    if (IsNull(combovalue) == false) {
        var data = "id=" + combovalue + "&";
        Ext.Object.each(innerform.getValues(), function (n, v) {
            data += "ConditionName=" + n + "&";
            data += "ConditionValue=" + v + "&";
        });
        data += "1=1";
        $.post(FF_GetCookie("VirtualDir") + '/WebApi/Home/SaveCondition', data, function (data) {
            fn();
        })
    }
}

function FF_CommonConditionDelete(cccomboid, fn) {
    var cc = Ext.getCmp(cccomboid);
    var innerform = cc.up('form');
    var combovalue = cc.getValue();
    if (IsNull(combovalue) == false) {
        var data = "id=" + combovalue;
        $.post(FF_GetCookie("VirtualDir") + '/WebApi/Home/DeleteCondition', data, function (data) {
            cc.store.remove(cc.store.getById(combovalue));
            cc.setValue('null', true);
            fn();
        })
    }
}

function FF_ComboAfterrender(self) {
    var comboValue = new Array();
    Ext.Array.each(self.valueIndex, function (val, index) {
        try {
            var v = self.store.data.items[val - 1].data['v'];
            if (v != null && v != '') {
                comboValue.push(v);
            }
        }
        catch (e) {
        }
    });
    if (comboValue != null && comboValue.length > 0) {
        self.setValue(comboValue);
    }
}

function FF_Alert(title, msg) {
    Ext.Msg.alert(title, msg);
}

function FF_Promot(title, msg, fn) {
    Ext.Msg.prompt(title, msg, fn);
}

function FF_OpenSimpleDialog(title, message) {
    Ext.Msg.alert(title, message);
}

function FF_MessageBox(title, msg, btns, icon, fn) {
    Ext.Msg.show({
        title: title,
        message: msg,
        buttons: btns,
        icon: icon,
        fn: fn
    });
}

function FF_DownloadExcelOrPdfPost(gridid, url) {
    var grid = Ext.getCmp(gridid);
    if (grid == null) {
        FF_CloseMask();
        return;
    }
    var sels = grid.getSelectionModel().getSelection();
    var html = "";
    var newForm = $('<form>');
    newForm.attr("action", FF_GetCookie("VirtualDir") + url);
    newForm.attr("method", "post");
    newForm.attr("target", "DONOTUSE_HIDDENPOSTIF");
    var html = "<input type=\"hidden\" name = \"FFVmName\" value=\"" + grid.store.proxy.extraParams['FFVmName'] + "\" />";
    if (sels.length == 0) {
        var btn = Ext.getCmp(grid.triggerBtnID);
        if (btn != null) {
            var innerform = btn.up('form');
            if (innerform != null) {
                if (innerform.hasInvalidField() == true) {
                    FF_CloseMask();
                    innerform.isValid();
                    return;
                }
                else {
                    innerform.isValid();
                }
                Ext.Object.each(innerform.getValues(), function (n, v) {
                    if ($.isArray(v)) {
                        $.each(v, function (n1, v1) {
                            if (v1 == "null") {
                                v1 = "";
                            }
                            html += "<input type=\"hidden\"  name = \"" + n + "\" value=\"" + v1 + "\" />";
                        });
                    }
                    else {
                        if (v == "null") {
                            v = "";
                        }
                        html += "<input type=\"hidden\"  name = \"" + n + "\" value=\"" + v + "\" />";
                    }
                });
            }
        }
    }
    else {
        var regex = /(<([^>]+)>)/ig;
        $.each(sels, function (n, v) {
            html += "<input type=\"hidden\"  name = \"IDs\" value=\"" + v.data.ID.replace(regex, "") + "\" />";
        });
    }
    newForm.html(html);
    $("body").append(newForm);
    $.cookie("DONOTUSEDOWNLOADING", "1", { path: '/' });
    newForm.submit();
    newForm.remove();
    _delayclose();
}

function FF_DownloadTemplatePost(url, FFVmFullName, formID, gridID) {
    var html = "";
    var innerform = Ext.getCmp(formID);
    if (innerform != null) {
        if (innerform.hasInvalidField() == true) {
            FF_CloseMask();
            innerform.isValid();
            return;
        }
        else {
            innerform.isValid();
        }
        Ext.Object.each(innerform.getValues(), function (n, v) {
            if ($.isArray(v)) {
                $.each(v, function (n1, v1) {
                    if (v1 == "null") {
                        v1 = "";
                    }
                    html += "<input type=\"hidden\"  name = \"" + n + "\" value=\"" + v1 + "\" />";
                });
            }
            else {
                if (v == "null") {
                    v = "";
                }
                html += "<input type=\"hidden\"  name = \"" + n + "\" value=\"" + v + "\" />";
            }
        });
    }
    var ids = FF_GetSelectIDs(gridID);
    if (ids != null && ids.length > 0) {
        html += "<input type=\"hidden\"  name = \"IDs\" value=\"" + ids + "\" />";
    }
    var newForm = $('<form>');
    newForm.attr("action", FF_GetCookie("VirtualDir") + url);
    newForm.attr("method", "post");
    newForm.attr("target", "DONOTUSE_HIDDENPOSTIF");

    html += "<input type=\"hidden\" name = \"FFVmFullName\" value=\"" + FFVmFullName + "\" />";
    newForm.html(html);
    $("body").append(newForm);
    $.cookie("DONOTUSEDOWNLOADING", "1", { path: '/'});
    newForm.submit();
    newForm.remove();
    _delayclose();

}

function _delayclose() {
    if ($.cookie("DONOTUSEDOWNLOADING") != "0") {
        setTimeout(function dd() { _delayclose(); }, 500);
    }
    else {
        FF_CloseMask();
    }
}

function FF_DownloadAttachment(fileID) {
    var url = FF_GetCookie("VirtualDir") + "/WebApi/Home/DownloadAttachment/" + fileID;
    window.open(url, 'DONOTUSE_HIDDENPOSTIF');
    return false;
}

function FF_AttachmentView(fileID, title, width, height, resizable) {
    var url = FF_GetCookie("VirtualDir") + "/WebApi/Home/ViewAttachment/" + fileID;
    FF_OpenDialog(url, title, width, height, undefined, undefined, undefined, undefined, resizable);
    return false;
}

function FF_ShowCenter() {
    $("#FileObject").parents(".x-window").each(function (n, v) {
        var id = $(v).attr("id");
        if (id != null && id != "") {
            Ext.getCmp(id).center();
        }
    });
}

function ParseBool(s) {
    if (s.toLowerCase() == "true") {
        return true;
    }
    else {
        return false;
    }
}

function IsNull(val) {
    if (val == undefined || val == null || val == '' || val == 'null') {
        return true;
    }
    else {
        return false;
    }
}

function FF_DeactiveAllMenuItem() {
    Ext.getCmp('DONOTUSE_MAINMENU').items.each(function (item, index, length) {
        var sel = item.getSelectionModel();
        sel.deselectAll(true);
    });
}

function FF_SetActiveMenu(url, did) {
    if (DONOTUSE_MENUREADY == false) {
        setTimeout(function delaysetactivemenu() { FF_SetActiveMenu(url); }, 500);
        return;
    }
    surl = new String(url);
    index = surl.indexOf("?");
    if (index > 0) {
        url = surl.substr(0, index);
    }
    var menu = Ext.getCmp('DONOTUSE_MAINMENU');
    var nood = null;
    if (menu.xtype == "panel") {
        menu.items.each(function (item, index, length) {
            var rootnood = item.getRootNode();
            nood = FF_FindNode(rootnood, url, did);
            if (nood != null) {
                setTimeout(function tempexpand() {
                    item.expand();
                }, 200);
                item.getSelectionModel().select(nood, false, true);
                return false;
            }
        });
    }
    if (menu.xtype == "treepanel") {
        var rootnood = menu.getRootNode();
        nood = FF_FindNode(rootnood, url, did);
        if (nood != null) {
            menu.getSelectionModel().select(nood, false, true);
            return false;
        }
    }
    if (nood == null) {
        if (menu.getSelectionModel != undefined && menu.getSelectionModel() != null) {
            menu.getSelectionModel().deselectAll();
        }
    }
}

function FF_FindNode(root, url, did) {
    var rv = root.findChildBy(function fn(node) {
        if (node.data.url != null && url != null && node.data.url.toLowerCase() == url.toLowerCase() && node.data.domainid == did) {
            return true;
        }
    });
    if (rv == null && root.childNodes != null && root.childNodes.length > 0) {
        $.each(root.childNodes, function (n, v) {
            rv = FF_FindNode(v, url, did);
            if (rv != null) {
                return false;
            }
        });
    }
    if (rv != null) {
        setTimeout(function tempexpand() {
            root.expand();
        }, 200);
    }
    return rv;
}

function FF_DetailGridCreate(gridID, defModel) {
    var _grid = Ext.getCmp(gridID);
    var rowEditing = _grid.plugins != null && _grid.plugins.length > 0 ? _grid.plugins[0] : null;
    if (rowEditing != null) rowEditing.cancelEdit();
    var store = _grid.getStore();
    var modelName = store.getModel().getName();
    var r = Ext.create(modelName, defModel[0]);
    var guid = FF_uuid();
    r.data.ID = guid;
    r.id = guid;
    store.insert(0, r);
    if (rowEditing != null) rowEditing.startEdit(0);
}

function FF_DetailGridDelete(gridID) {
    var _grid = Ext.getCmp(gridID),
    rowEditing = _grid.plugins != null && _grid.plugins.length > 0 ? _grid.plugins[0] : null,
    _store = _grid.getStore(),
    sm = _grid.getSelectionModel();
    if (rowEditing != null) rowEditing.cancelEdit();
    setTimeout(function tempexpand() {
        if (sm.getSelection().length > 0) {
            _store.remove(sm.getSelection());
            if (_store.getCount() > 0) {
                sm.select(0);
            }
        }
    }, 100);

}

function FF_ViewDFSPic(url, title, width, height, resizeable) {
    Ext.create('Ext.window.Window', {
        title: IsNull(title) ? '查看' : title,
        width: width,
        height: height,
        resizable: resizeable,
        html: '<img src=\'' + url + '\'/>'
    }).show();
}

function FF_DetailGridDialog(gridID, url, title, width, height, did, cid, resizeable, bindid) {
    var _grid = Ext.getCmp(gridID),
    rowEditing = _grid.plugins != null && _grid.plugins.length > 0 ? _grid.plugins[0] : null,
    _store = _grid.getStore(),
    sm = _grid.getSelectionModel();

    if (rowEditing != null) rowEditing.cancelEdit();
    setTimeout(function tempexpand() {
        if (sm.getSelection().length > 0) {
            var data = sm.getSelection();
            if (url.indexOf("?") < 0) {
                url = url + "?1=1";
            }
            var d = data[0].data[bindid];
            if (d != "") {
                url = url + "&id=" + d;
                FF_OpenDialog(url, title, width, height, "", "", did, cid, resizeable);
            }
        }
    }, 100);
}

function FF_GetSystemNotices() {
    $.ajax({
        type: 'GET',
        url: FF_GetCookie("VirtualDir") + '/WebApi/Home/GetSystemNotices',
        success: function (data) {
            data = eval(data);
            if (data != null && data instanceof Array && data.length > 0) {
                var loopTime = Math.ceil(data.length / 5);

                for (var i = 0; data.length > 0 && i < 5; i++) {
                    Ext.create({
                        xtype: 'uxNotification',
                        useXAxis: true,
                        //cls: 'ux-notification-light',
                        position: 't',
                        title: data[0].Title == null ? "系统通知" : data[0].Title,
                        html: data[0].Content,
                        slideInAnimation: 'bounceOut',
                        slideBackAnimation: 'easeIn',
                        width: data[0].Width == 0 ? 250 : data[0].Width,
                        height: data[0].Height == 0 ? 130 : data[0].Height,
                        autoClose: data[0].StickTime != 0,
                        autoCloseDelay: data[0].StickTime * 1000
                    }).show();
                    data.remove(0);
                }
                loopTime--;

                var interID = setInterval(function () {
                    if (loopTime <= 0) {
                        clearInterval(interID);
                    }
                    else {
                        for (var i = 0; data.length > 0 && i < 5; i++) {
                            Ext.create({
                                xtype: 'uxNotification',
                                useXAxis: true,
                                //cls: 'ux-notification-light',
                                position: 't',
                                title: data[0].Title == null ? "系统通知" : data[0].Title,
                                html: data[0].Content,
                                slideInAnimation: 'bounceOut',
                                slideBackAnimation: 'easeIn',
                                width: data[0].Width == 0 ? 250 : data[0].Width,
                                height: data[0].Height == 0 ? 130 : data[0].Height,
                                autoClose: data[0].StickTime != 0,
                                autoCloseDelay: data[0].StickTime * 1000
                            }).show();
                            data.remove(0);
                        }
                    }
                    loopTime--;
                }, 15000);
            }
        }
    });
}

function FF_GetSelectIDs(vmName) {
    var ids = "";  /*拼接选择的ID结果表*/
    var grid = Ext.getCmp(vmName);
    if (grid != null && grid.getSelectionModel() != null) {
        var gridList = grid.getSelectionModel().getSelection();
        var lengths = gridList.length;
        for (var i = 0; i < lengths; i++) {
            if (i < lengths - 1) {
                ids += gridList[i].data.ID + ",";
            }
            else {
                ids += gridList[i].data.ID;
            }
        }
        return ids;
    }
    return ids;
}

/*
 * 数组扩展函数
 * 按索引移除数据
 */
Array.prototype.remove = function (index) {
    if (isNaN(index) || index >= this.length) {
        return false;
    }
    for (var i = 0, j = 0; i < this.length; i++) {
        if (i != index) {
            this[j] = this[i];
            j++;
        }
    }
    this.length--;
}

/*
 * 数组扩展函数
 * 将数据插入到指定位置
 */
Array.prototype.insertAt = function (index, value) {
    if (isNaN(index)) {
        return this;
    }
    var part1 = this.slice(0, index);
    var part2 = this.slice(index);
    part1.push(value);
    return (part1.concat(part2));
};

function FF_uuid() {

    var s = [];

    var hexDigits = "0123456789abcdef";

    for (var i = 0; i < 36; i++) {

        s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);

    }

    s[14] = "4";  /* bits 12-15 of the time_hi_and_version field to 0010*/

    s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);  /* bits 6-7 of the clock_seq_hi_and_reserved to 01*/

    s[8] = s[13] = s[18] = s[23] = "-";



    var uuid = s.join("");

    return uuid;

}
/*自定义Vtype:'Data' 日期*/
Ext.apply(Ext.form.field.VTypes, {
    Data: function (val, field) {
        return (/^\d{4}(\-|\/|.)(0?[1-9]|1[0-2])\1(0?[1-9]|1[0-2])$/).test(val);
    },
    DataText: '不是有效的日期类型。',
    DataMask: /[\d-]/i
});
/*自定义Vtype:'Time' 时间*/
Ext.apply(Ext.form.field.VTypes, {
    Time: function (val, field) {
        return (/(^([0-1][0-9]|2[0-3]):[0-5][0-9](:[0-5][0-9])?$)|(^([1-9]|1[0-2]):[0-5][0-9](:[0-5][0-9])?\s?[a|p]m$)/i).test(val);
    },
    TimeText: '不是有效的时间类型。',
    TimeMask: /[\d\s:amp]/i
});
/*自定义Vtype:'IPaddress' IP地址*/
Ext.apply(Ext.form.field.VTypes, {
    IPaddress: function (v) {
        return (/^((?:(?:25[0-5]|2[0-4]\d|[01]?\d?\d)\.){3}(?:25[0-5]|2[0-4]\d|[01]?\d?\d))$/).test(v);
    },
    IPaddressText: '不是有效的IPv4地址',
    IPaddressMask: /[\d\.]/i
});
/*自定义Vtype:'Int' 整数*/
Ext.apply(Ext.form.field.VTypes, {
    Int: function (v) {
        return (/^-?\d*$/).test(v);
    },
    IntText: '不是有效的整数类型',
    IntMask: /[\d-]/i
});
/*自定义Vtype:'UInt' 正整数*/
Ext.apply(Ext.form.field.VTypes, {
    UInt: function (v) {
        return (/^\d*$/).test(v);
    },
    UIntText: '不是有效的整数类型',
    UIntMask: /[\d]/i
});
/*自定义Vtype:'Double' 浮点数*/
Ext.apply(Ext.form.field.VTypes, {
    Double: function (v) {
        return (/^-?\d+\.?\d+$/).test(v);
    },
    DoubleText: '不是有效的浮点数类型',
    DoubleMask: /[\d\.-]/i
});
/*自定义Vtype:'UDouble' 正浮点数*/
Ext.apply(Ext.form.field.VTypes, {
    UDouble: function (v) {
        return (/^\d+\.?\d+$/).test(v);
    },
    UDoubleText: '不是有效的浮点数类型',
    UDoubleMask: /[\d\.]/i
});
/*自定义Vtype:'Color' 十六进制色值*/
Ext.apply(Ext.form.field.VTypes, {
    Color: function (v) {
        return (/^#[a-fA-F0-9]{6}$/).test(v);
    },
    ColorText: '不是有效的十六进制色值',
    ColorMask: /[a-fA-F\d#]/i
});
/*自定义Vtype:'Phone' 手机号*/
Ext.apply(Ext.form.field.VTypes, {
    Phone: function (v) {
        return (/^0?(13|14|15|18|17)[0-9]{9}$/).test(v);
    },
    PhoneText: '不是有效的手机号',
    PhoneMask: /[\d]/i
});
/*自定义Vtype:'Tel' 电话号码*/
Ext.apply(Ext.form.field.VTypes, {
    /*vtype 校验函数*/
    Tel: function (v) {
        return (/(^[1-9]\d{6,7}$)|(^[1-9]\d{6,7}-\d{1,4}$)|(^\d{3}\d?-[1-9]\d{6,7}$)|(^\d{3}\d?-[1-9]\d{6,7}-\d{1,4}$)|(^\(\d{3}\d?\)[1-9]\d{6,7}$)|(^\(\d{3}\d?\)[1-9]\d{6,7}-\d{1,4}$)/).test(v);
    },
    /*vtype文本属性：当验证函数返回false显示的出错文本*/
    TelText: '不是有效的电话号码',
    /*vtype Mask 属性: 按键过滤器*/
    TelMask: /[\d-()]/i
});
