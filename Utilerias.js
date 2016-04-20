"use strict";
var modoDebug = false;

Object.defineProperty(Number.prototype, "min32", { get: function myProperty() { return -2147483647; } });
Object.defineProperty(Number.prototype, "max32", { get: function myProperty() { return 2147483648; } });

function crearListaHtml(arreglo, listaOrdenada) {
    if (listaOrdenada) {
        return "<ol><li>".concat(arreglo.join("</li><li>"), "</li></ol>")
    }
    else {
        return "<ul><li>".concat(arreglo.join("</li><li>"), "</li></ul>")
    }
}

function ObtenerOpciones(select) { /// <summary> Obtiene un arreglo de los options que estan en un select </summary>
    var opciones = [];
    var children = $(select).children("option");
    for (var i = 0, length = children.length; i < length; i++) {
        var item = children[i];
        opciones.push({ Text: item.text, Value: item.value });
    }
    return opciones;
}

function showDialog(titulo, mensaje, modal, botones) { /// <summary> Muestra un mensaje al usuario </summary>
    var div = $("<div style='padding:0.3em;'/>");
    div.html(mensaje);
    if (_.isEmpty(botones)) {
        botones = {}
        botones[recursos.Cerrar] = closeDialog;
    }
    return div.dialog({ modal: modal, title: titulo, buttons: botones });
}

// Ejecuta callback en caso de estar definido y cierra el dialog del contexto actual 
function closeDialog(callback) {
    function llamada() {
        if (_.isFunction(callback)) {
            callback();
        }
        else {
            console.warn("El parámetro callback no es una función válida", callback);
        }
        $(this).dialog("close");
    }
    if (_.isFunction(callback)) {
        return llamada;
    }
    else {
        $(this).dialog("close");
    }
}

function mostrarError(jqxhr, textStatus, errorThrown) { /// <summary> Muestra el mensaje de error al usuario en modal </summary>
    error(arguments);
    showDialog(mensajes.errorTitulo, mensajes.errorInesperado, true);
}

function mostrarTooltipsValidaciones() {
    // pone en toolTips los mensajes de validacion
    $(document).tooltip({
        items: ".input-validation-error",
        content: getContent
    });

    function getContent() {
        return $("span".concat("[data-valmsg-for=", $(this).id(), "] span")).text().coalesce(mensajes.incorrecto);
    }
}

function medirTiempoEjecucion(funcion) {
    var inicio = new Date();
    funcion();
    var fin = new Date();
    info("medirTiempoEjecucion : ".concat(fin - inicio, " ms"));
}
// bloquea la pantalla mientras se descarga un archivo. devuelve el nombre de una cookie la cual deberá cambiar en cada llamada a la acción que despacha el archivo
function bloquearPorDescarga() {
    $.blockUI();
    var cookie = Math.random().toString();
    var galleta = Cookies.get(cookie);
    var intervalo = setInterval(verificarDescarga, 1000);

    function verificarDescarga() {
        if (galleta != Cookies.get(cookie)) {
            clearInterval(intervalo);
            Cookies.remove(cookie);
            $.unblockUI();
        }
    }
    return cookie;
}

function windowOpen(url, objectParams, nombre, esModal) {   /// <summary> Abre una nueva ventana con la url especificada y sus parametros en modo GET </summary>
    if (objectParams) {
        var params = [];
        for (var prop in objectParams) {
            params.push(prop.concat("=", objectParams[prop]));
        }
        url = url.concat("?", params.join("&"));
    }
    var modal = esModal ? "modal=yes" : undefined;
    if (nombre || esModal) {
        return window.open(url, nombre, modal);
    }
    else {
        return window.open(url);
    }
}

function windowOpenFeats(url, objectParams, nombre, features) {   /// <summary> Abre una nueva ventana con la url especificada y sus parametros en modo GET </summary>
    if (objectParams) {
        var params = [];
        for (var prop in objectParams) {
            params.push(prop.concat("=", objectParams[prop]));
        }
        url = url.concat("?", params.join("&"));
    }
    if (nombre || features) {
        return window.open(url, nombre, features);
    }
    else {
        return window.open(url);
    }
}

/* Abre una nueva ventana con la url especificada y sus parametros en modo POST 
target:
    _blank - URL is loaded into a new window. This is default
    _parent - URL is loaded into the parent frame
    _self - URL replaces the current page
    _top - URL replaces any framesets that may be loaded
    name - The name of the window (Note: the name does not specify the title of the new window)
*/
function windowOpenPost(url, objectParams, target) {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", url);

    var nombre = target != null ? target : "vista" + Math.random().toString().substring(2);
    form.setAttribute("target", nombre);

    for (var prop in objectParams) {
        var hiddenField = document.createElement("input");
        hiddenField.setAttribute("type", "hidden");
        hiddenField.setAttribute("name", prop);
        hiddenField.setAttribute("value", objectParams[prop]);
        form.appendChild(hiddenField);
    }

    document.body.appendChild(form);

    form.submit();
    document.body.removeChild(form);
}

/*================================================================
                 funciones para depuracion
==================================================================*/
function empty() { }

if (console == undefined) {
    console = {
        log: empty,
        info: empty,
        warn: empty,
        error: empty,
        count: empty,
        assert: empty
    };
}

// si existe modoDebug, loguea el objeto en la consola
function log() {
    if (modoDebug && window.console) {
        var a = arguments;
        switch (a.length) {
            case 1: return console.log(a[0]);
            case 2: return console.log(a[0], a[1]);
            case 3: return console.log(a[0], a[1], a[2]);
            case 4: return console.log(a[0], a[1], a[2], a[3]);
            case 5: return console.log(a[0], a[1], a[2], a[3], a[4]);
            case 6: return console.log(a[0], a[1], a[2], a[3], a[4], a[5]);
            case 7: return console.log(a[0], a[1], a[2], a[3], a[4], a[5], a[6]);
            case 8: return console.log(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7]);
            case 9: return console.log(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8]);
            case 10: return console.log(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8], a[9]);
            default: return console.log(a);
        }
    }
}
// si existe modoDebug, muestra el mensaje de información del objeto en la consola
function info() {
    if (modoDebug && window.console) {
        var a = arguments;
        switch (a.length) {
            case 1: return console.info(a[0]);
            case 2: return console.info(a[0], a[1]);
            case 3: return console.info(a[0], a[1], a[2]);
            case 4: return console.info(a[0], a[1], a[2], a[3]);
            case 5: return console.info(a[0], a[1], a[2], a[3], a[4]);
            case 6: return console.info(a[0], a[1], a[2], a[3], a[4], a[5]);
            case 7: return console.info(a[0], a[1], a[2], a[3], a[4], a[5], a[6]);
            case 8: return console.info(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7]);
            case 9: return console.info(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8]);
            case 10: return console.info(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8], a[9]);
            default: return console.info(a);
        }
    }
}
// Muestra el mensaje de advertencia del objeto en la consola
function warn() {
    if (window.console) {
        var a = arguments;
        switch (a.length) {
            case 1: return console.warn(a[0]);
            case 2: return console.warn(a[0], a[1]);
            case 3: return console.warn(a[0], a[1], a[2]);
            case 4: return console.warn(a[0], a[1], a[2], a[3]);
            case 5: return console.warn(a[0], a[1], a[2], a[3], a[4]);
            case 6: return console.warn(a[0], a[1], a[2], a[3], a[4], a[5]);
            case 7: return console.warn(a[0], a[1], a[2], a[3], a[4], a[5], a[6]);
            case 8: return console.warn(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7]);
            case 9: return console.warn(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8]);
            case 10: return console.warn(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8], a[9]);
            default: return console.warn(a);
        }
    }
}
// Muestra el mensaje de error del objeto en la consola
function error() {
    if (window.console) {
        var a = arguments;
        switch (a.length) {
            case 1: return console.error(a[0]);
            case 2: return console.error(a[0], a[1]);
            case 3: return console.error(a[0], a[1], a[2]);
            case 4: return console.error(a[0], a[1], a[2], a[3]);
            case 5: return console.error(a[0], a[1], a[2], a[3], a[4]);
            case 6: return console.error(a[0], a[1], a[2], a[3], a[4], a[5]);
            case 7: return console.error(a[0], a[1], a[2], a[3], a[4], a[5], a[6]);
            case 8: return console.error(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7]);
            case 9: return console.error(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8]);
            case 10: return console.error(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8], a[9]);
            default: return console.error(a);
        }
    }
}
// si existe modoDebug, muestra el mensaje del conteo del objeto en la consola
function count(nombre) {
    if (modoDebug && window.console) {
        console.count(nombre);
    }
}
// si existe modoDebug, muestra el mensaje del conteo del objeto en la consola
function assert(condicion, texto) {
    if (modoDebug && window.console) {
        console.assert(condicion, texto);
    }
}
// muestra en la consola los datos en forma tabular. Útil para arreglos de objetos
function logTable(tabularData) {
    if (modoDebug && console.table) {
        console.table(tabularData);
    }
}
// Agrupa las siguientes entradas a la consola
function logGroup(nombre) {
    if (modoDebug && console.group) {
        console.group(nombre);
    }
}
// termina el grupo actual iniciado por console.group
function logGroupEnd() {
    if (modoDebug && console.groupEnd) {
        console.groupEnd();
    }
}
// Inicia un conteo con la etiqueta para medir el tiempo 
function logTime(etiqueta) {
    if (modoDebug && console.time) {
        console.time(etiqueta);
    }
}
// termina el conteo con la etiqueta y obtiene los milisegundos que pasaron desde la llamada de la función time
function logTimeEnd(etiqueta) {
    if (modoDebug && console.timeEnd) {
        console.timeEnd(etiqueta);
    }
}

function jqGridBeforeProcessingLogError(data, status, xhr) {
    if (data.Error == true) {
        error(data.Mensaje);
    }
}

/*================================================================
                        funciones generales
==================================================================*/
//Clona completamente un objeto a otro, con referencias distintas a todos los objetos.
function clonar(obj) {
    return JSON.parse(JSON.stringify(obj));
}
Number.prototype.round = function (decimales) {/// <summary>Redondea el numero a la cantidad de decimales especificados</summary>
    places = Math.pow(10, places);
    return Math.round(this * decimales) / decimales;
};

String.prototype.removeAll = function (text) {/// <summary>Elimina todas las apariciones y devuelve el texto generado</summary>
    return this.split(text).join("");
}
String.prototype.replaceAll = function (value, replacement) {/// <summary>Reemplaza todas las apariciones y devuelve el texto generado</summary>
    return this.split(value).join(replacement);
}
Date.prototype.toIsoDate = function () {/// <summary>Convierte a texto con el formato ISO yyyy-MM-dd donde yyyy es el año, MM es el número del mes y dd es el día del mes</summary>
    return this.toISOString().substring(0, 10);
};
Date.prototype.getDateOnly = function () {
    return new Date(this.getFullYear(), this.getMonth(), this.getDate());
};
Date.prototype.minValue = new Date(0, 0, 1);

String.prototype.coalesce = function () {/// <summary>Si el valor actual es nulo, vacío o solo consiste por espacios en blanco, devuelve algun parámetro en orden que no sea nulo, vacío ni que consista solo por espacios en blanco</summary>

    if (EsNuloVacioEspaciosBlanco(this)) {
        for (var i = 0, max = arguments.length; i < max; i++) {
            if (!EsNuloVacioEspaciosBlanco(arguments[i])) {
                return arguments[i];
            }
        }
    }
    return this;

    function EsNuloVacioEspaciosBlanco(texto) {
        return texto == null || texto.trim && texto.trim() == "" || texto.toString().trim() == "";
    }
};
String.prototype.format = function () {
    var argsFormat = arguments;
    function replace(match, number) {
        return typeof argsFormat[number] != 'undefined' ? argsFormat[number] : match;
    }
    return this.replace(/{(\d+)}/g, replace);
};
function dateStrToDate(objeto) {/// <summary>Convierte a Date el texto con el formato /Date(############)/ </summary>
    // /\d+/.exec("\/Date(1293861600000)\/")
    return _.isDate(objeto)
        ? objeto
        : vacio(objeto)
        ? null
        : /Date\(\d+\)/.test(objeto)
        ? new Date(Number(_.first(/\d+/.exec(objeto))))
        : objeto;
}
function vacio(str) {/// <summary> Devuelve true si el valor actual es indefinido, nulo, vacío o solo consiste por espacios en blanco</summary>
    return str == undefined || str == null || str === "" || str.toString().trim() == "";
}

function diccionar(arreglo, llave, valor) {
    var diccionario = {};
    for (var index in arreglo) {
        diccionario[arreglo[index][llave]] = arreglo[index][valor];
    }
    return diccionario;
}

function sum(coleccion, propiedad) {
    var suma = 0;
    for (var i = 0; i < coleccion.length; i++) {
        suma += Number(coleccion[i][propiedad]);
    }
    return suma;
}

function keydownHandler(e) {

    var key = e.which || e.keyCode;

    if (//!e.shiftKey && !e.altKey && !e.ctrlKey &&
        e.shiftKey || e.altKey || e.ctrlKey ||
        // numbers   
        key >= 48 && key <= 57 ||
        // Numeric keypad
        key >= 96 && key <= 105 ||
        // Backspace and Tab and Enter
        key == 8 || key == 9 || key == 13 ||
        // Home and End
        key == 35 || key == 36 ||
        // left and right arrows
        key == 37 || key == 39 ||
        // Del and Ins
        key == 46 || key == 45)
        return true;

    return false;
}
//devuelve el primer parámetro que no sea nulo de todos los que haya
function coalesce() {
    for (var i in arguments) {
        if (arguments[i] != null) {
            return arguments[i];
        }
    }
    return null;
}

function isNull(valor, sustituto) {/// <summary> Si el valor es nulo o indefinido, devuelve el sustituto, de lo contrario devuelve el valor</summary>
    return valor != undefined && valor != null ? valor : sustituto;
}

function isNullOrEmpty(valor, sustituto) {/// <summary> Si el valor es nulo, indefinido o es un texto que consiste en espacios vacíos, devuelve el sustituto, de lo contrario devuelve el valor</summary>
    return vacio(valor) ? sustituto : valor;
}

function dateParseFormat(string, formato) {
    var separador = _.first(/\W/.exec(formato));
    if (!vacio(string) && !_.isUndefined(separador)) {
        var form = formato.split(separador);
        var val = string.split(separador);
        return new Date(val[form.indexOf("y")], val[form.indexOf("m")] - 1, val[form.indexOf("d")]);
    }
}
/*================================================================
                               JQuery
==================================================================*/
// si la propieddad formatter existe en la columna, se utiliza para crear la propieadad sorttype en caso de no existir
function defaultSort(colModel) {
    colModel.forEach(iteracion);
    function iteracion(col) {
        if (vacio(col.sorttype) && !vacio(col.formatter)) {
            col.sorttype = col.formatter;
        }
    }
}

jQuery.fn.num = function () {
    return Number(this.val().replace(/[^\.\d]/g, ""));
}

jQuery.fn.id = function () {
    return this.attr("id");
}

jQuery.fn.changeDelayed = function (funcion, timeout) {
    function delayed() {
        setTimeout(funcion, timeout, this);
    }
    return this.change(delayed)
}

jQuery.fn.forceInteger = function () {
    function validar32bits(texto) {
        var digitos = isNull(_.first(texto.match(/\d+/)), "");
        return Number(digitos) > Integer.max ? isNull(_.first(/1?\d{1,9}/.exec(texto)), "") : digitos;
    }
    function pasteHandler(e) {
        function filtrar() {
            if (e.currentTarget.type == "number") {
                warn("ForceInteger: La extracción de dígitos a partir de un texto solo es compatible con inputs de tipo text");
            }
            var input = $(e.currentTarget);
            input.val(validar32bits(input.val())).change();//el change lo requieren algunas librerias javascript
        }
        setTimeout(filtrar, 100);
    }
    $(this).on("paste", pasteHandler);

    $(this).keydown(keydownHandler);

    function focusoutHandler(e) {
        $(e.currentTarget).val(validar32bits(e.currentTarget.value)).change();//el change lo requieren algunas librerias javascript
    }
    $(this).focusout(focusoutHandler);
}

jQuery.fn.forceNumeric = function () {
    var filtrando = false;
    $(this).change(function (e) {
        if (filtrando == false) {
            filtrando = true;
            e.target.value = e.target.value.replace(/[^\d\.]/g, "");
            log(e.target.value);
            $(e.target).val(Number(isNull(_.first(/\d+(\.\d+)?/.exec(e.target.value)), ""))).change();
            filtrando = false;
        }
    });

    function pasteHandler(e) {
        function filtrar(e) {
            if (e.currentTarget.type == "number") {
                warn("ForceNumeric: La extracción de dígitos a partir de un texto solo es compatible con inputs de tipo text");
            }
            var input = $(e.currentTarget);
            input.val(isNull(_.first(/\d+(\.\d+)?/.exec(input.val())), "")).change();//el change lo requieren algunas librerias javascript
        }
        setTimeout(filtrar, 100, e);
    }
    $(this).on("paste", pasteHandler);

    $(this).keydown(keyDownHandler);
    function keyDownHandler(e) {
        var key = e.which || e.keyCode;

        return e.shiftKey || e.altKey || e.ctrlKey ||
                // numbers   
                key >= 48 && key <= 57 ||
                // Numeric keypad
                key >= 96 && key <= 105 ||
                // comma, period and minus, . on keypad
                key == 190 || key == 188 || key == 109 || key == 110 ||
                // Backspace and Tab and Enter
                key == 8 || key == 9 || key == 13 ||
                // Home and End
                key == 35 || key == 36 ||
                // left and right arrows
                key == 37 || key == 39 ||
                // Del and Ins
                key == 46 || key == 45;
    }
}

jQuery.fn.enabled = function (enabled) {
    if (enabled == undefined) {
        return !$(this).prop("disabled");
    }
    return $(this).prop("disabled", !enabled);
}

jQuery.fn.checked = function (checked) {
    if (checked == undefined) {
        return $(this).prop("checked");
    }
    return $(this).prop("checked", checked);
}

jQuery.fn.readonly = function (readonly) {
    if (readonly == undefined) {
        return $(this).prop("readonly");
    }
    return $(this).prop("readonly", readonly);
}

jQuery.fn.selRow = function () {
    var id = $(this).jqGrid('getGridParam', 'selrow');
    return $(this).jqGrid('getRowData', id);
}

jQuery.fn.selArrRows = function () {
    var ids = $(this).jqGrid('getGridParam', 'selarrrow');
    var array = [];
    for (var i = 0, length = ids.length; i < length; i++) {
        array.push($(this).jqGrid('getRowData', ids[i]));
    }
    return array;
}

jQuery.fn.date = function (newDate) {
    return newDate == undefined
        ? $(this).datepicker("getDate")
        : $(this).datepicker("setDate", newDate);
}

jQuery.fn.toJS = function () {
    var arr = $(this).find("input, select, textarea");
    var obj = {}, item, id;
    for (var i = 0; i < arr.length; i++) {
        item = $(arr[i])
        id = item.id();
        if (id != null) {
            obj[id] = item.val();
        }
        else {
            warn("JQuery.fn.toJS : El control {0} no tiene un ID asignado".format(arr[i]));
        }
    }
    return obj;
}

jQuery.fn.aplicarMoneda = function (options) {//<summary> Aplica el estilo de moneda a input[type="text"]</summary>
    var defaults = {
        css: 'moneda'
    };

    options = $.extend(defaults, options);

    var controls = this;
    controls.each(function () {
        var input = $(this).filter('input[type="text"]');
        var isFocused = input.is(":focus");

        input.unbind('blur focus');

        input.bind('blur', function () {
            if ($(this).val() != null && $(this).val() != "" && $.isNumeric($(this).val()))
                $(this).val(parseFloat($(this).val()).formatMoney(2, '.', ','));
        });
        input.bind('focus', function () {
            if ($(this).val() != null && $(this).val() != "")
                $(this).val($(this).val().replace(/,/g, ""));
        });
        if (input.val() != null && input.val() != "" && !isFocused && $.isNumeric(input.val())) {
            input.val(parseFloat(input.val()).formatMoney(2, '.', ','));
        }
        input.attr('maxlength', '15');
        input.css('text-align', 'right');

        //input.keypress(function (e) { return validMoney(e, $(this)); });
        input.forceNumeric();

        input.data("value", input.val());

        function aplicacion() {
            var isFocused = input.is(":focus");
            var data = input.data("value"),
                val = input.val();

            if (data !== val && !isFocused) {
                if (input.val() != null && input.val() != "" && $.isNumeric(val)) {
                    input.val(parseFloat(input.val()).formatMoney(2, '.', ','));
                }
                input.data("value", val);
            }
        }

        setTimeout(aplicacion, 1000);
    });

    return controls
};

jQuery.fn.removerMoneda = function (options) {//<summary> Remueve estilo de moneda a input[type="text"]</summary>
    var defaults = {
        css: 'moneda'
    };

    options = $.extend(defaults, options);

    var controls = this;
    controls.each(function () {
        var input = $(this).filter('input[type="text"]');
        var isFocused = input.is(":focus");

        if (input.val() != null && input.val() != "" && !isFocused) {
            input.val(input.val().replace(/,/g, ""));
        }
    });

    return controls
};

jQuery.fn.styleTable = function (options) {
    var defaults = { css: 'styleTable' };
    options = $.extend(defaults, options);

    return this.each(function () {

        var input = $(this);
        input.addClass(options.css);

        //input.find("tr").bind('mouseover mouseout', function (event) {
        //    if (event.type == 'mouseover') {
        //        $(this).children("td").addClass("ui-state-hover").css("cursor", "pointer");
        //    } else {
        //        $(this).children("td").removeClass("ui-state-hover");
        //    }
        //})

        input.find("th").addClass("ui-state-default ui-th-column");
        input.find("td").addClass("ui-widget-content");

        input.find("tr").each(function () {
            $(this).children("td:not(:first)").addClass("first");
            $(this).children("th:not(:first)").addClass("first");
        });
    });
};

$.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
    _title: function (title) {
        if (!this.options.title) {
            title.html("&#160;");
        } else {
            title.html(this.options.title);
        }
    }
}));
// agrega un tooltip acerca de los wildcard de sql like
jQuery.fn.sqlWildcardTooltip = function () {
    $(document).tooltip({
        items: this,
        content: $("#likewildcard").html(),
        show: 2000
    });
}

function loadingBlockUI(){
    $.blockUI.defaults.css = {
        padding: '6px 0',
        margin: '-18px 0 0 -83px',
        width: 128,
        height: 128,
        top: '50%',
        left: '50%',
        textAlign: 'center',
        color: 'black',
        backgroundColor: 'gray',
        cursor: 'wait'
    };
    $.blockUI.defaults.overlayCSS = {
        backgroundColor: '#AAAAAA',
        opacity: 0.3,
        cursor: 'wait'
    };
    $.blockUI.defaults.message = 'Cargando...';
    $.blockUI.defaults.fadeOut = 0;
    $.blockUI.defaults.fadeIn = 0;
    function blockUIValidado() {
        if (aplicacion.displayAjaxMsg) {
            $.blockUI();
        }
    }
    function desbloquearMostrarError(jqxhr, textStatus, errorThrown) {
        $.unblockUI();
        mostrarError(jqxhr, textStatus, errorThrown);
    }
    try
    {
        $(document).ajaxStart(blockUIValidado)
            .ajaxStop($.unblockUI)
            .ajaxError(desbloquearMostrarError);
    }
    catch (ex) {
        $.unblockUI();
        mostrarMensaje("Sistema Bonos", ex.message);
    }
}
// Borra el datepicker con el evento onClose
function clearDatepicker(dateText, inst) {
    if ($(window.event.srcElement).hasClass('ui-datepicker-close')) {
        document.getElementById(this.id).value = '';
    }
}

function scrollTo(control) {
    var element;
    if (_.isString(control)) {
        element = document.getElementById(control);
    }
    else (control.length != undefined && control.length > 0)
    {
        element = control[0];
    }
    if (element != null) {
        if (element.scrollIntoView != undefined) {
            element.scrollIntoView();
        }
        element.focus();
    }
}

function mostrarMensaje(titulo, mensaje, options, callBack) {
    //<summary>Muestra una ventana de mensaje</summary>
    if (mensaje == null) mensaje = "";
    if (titulo == null) titulo = "Sistema";

    if (options == null) options = new Object;
    if (callBack == null) callBack = function () { return false; }

    var width = options.width != null ? options.width : '350';
    var height = options.height != null ? options.height : 'auto';
    var autoOpen = options.autoOpen != null ? options.autoOpen : true;
    var resizable = options.resizable != null ? options.resizable : true;
    var modal = options.modal != null ? options.modal : true;

    var dialog = $("<div style='padding: .8em;'><div id='lblMensaje' ></div></div>").dialog({
        modal: modal,
        autoOpen: autoOpen,
        resizable: resizable,
        width: width,
        height: height,
        minHeight: '180',
        minWidth: '380',
        maxHeight: '90%',
        maxWidth: '90%',
        title: titulo,
        close: function (event, ui) {
            $(this).dialog("close");
            if (callBack != null) callBack();
            $(this).dialog("destroy");
        },
        buttons: {
            Cerrar: function () {
                $(this).dialog("close");
            }
        }
    });

    $("#lblMensaje").html(mensaje);

    return dialog;
};

function mostrarModal(titulo, content, options, callBack) {
    //<summary>Carga una ventana modal con el Html Content recibido</summary>
    if (titulo == null) titulo = "Mensaje";

    if (options == null) options = new Object;
    if (options.buttons == null) options.buttons =
    {
        Cerrar: function () {
            $(this).dialog("close");
            $(this).hide();
        }
    };

    var width = options.width != null ? options.width : 'auto';
    var height = options.height != null ? options.height : 'auto';
    var autoOpen = options.autoOpen != null ? options.autoOpen : true;
    var resizable = options.resizable != null ? options.resizable : true;
    var modal = options.modal != null ? options.modal : true;
    var minHeight = options.minHeight != null ? options.minHeight : '180';//: '180',
    var minWidth = options.minWidth != null ? options.minWidth : '380';//: '380',
    var maxHeight = options.maxHeight != null ? options.maxHeight : '90%';//: '90%',
    var maxWidth = options.maxWidth != null ? options.maxWidth : '90%';//: '90%',
    var beforeClose;
    if (options.beforeClose == null) {
        beforeClose =
        function (event, ui) {

        };
    }
    else {
        beforeClose = options.beforeClose;
    }

    var dialog = $(content).dialog({
        modal: modal,
        autoOpen: autoOpen,
        resizable: resizable,
        width: width,
        height: height,
        minHeight: minHeight,
        minWidth: minWidth,
        maxHeight: maxHeight,
        maxWidth: maxWidth,
        title: titulo,
        close: function (event, ui) {
            if (callBack != null) {
                callBack();
            }
            $(this).dialog("close");
            $(this).dialog("destroy");
        },
        beforeClose: beforeClose,
        buttons: options.buttons
    });

    return dialog;
};

function resizeIframe(obj) {
    obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
    obj.style.width = obj.contentWindow.document.body.scrollWidth + 'px';

    log({ height: obj.style.height, width: obj.style.width });
}

function mostrarDialog(titulo, url, options, callBack, iframe, objectParams) {
    //<summary>Carga una ventana modal con el Html Content recibido</summary>
    if (titulo == null) titulo = "Mensaje";

    if (options == null) options = {};
    if (callBack == null) callBack = function () { return false; }
    //if (options.buttons == null) options.buttons =
    //{
    //    Cerrar: function () {
    //        $(this).dialog("close");
    //    }
    //};

    var width = options.width != null ? options.width : 'auto';
    var height = options.height != null ? options.height : 'auto';
    var autoOpen = options.autoOpen != null ? options.autoOpen : true;
    var resizable = options.resizable != null ? options.resizable : true;
    var modal = options.modal != null ? options.modal : true;
    var minHeight = options.minHeight != null ? options.minHeight : '180';//: '180',
    var minWidth = options.minWidth != null ? options.minWidth : '380';//: '380',
    var maxHeight = options.maxHeight != null ? options.maxHeight : '90%';//: '90%',
    var maxWidth = options.maxWidth != null ? options.maxWidth : '90%';//: '90%',
    var close;
    if (options.close == null) {
        close =
        function (event, ui) {
            if (callBack != null) {
                callBack();
                $(this).dialog("close");
                $(this).dialog("destroy");
                $(this).remove();
            }
            else {
                $(this).dialog("close");
                $(this).dialog("destroy");
                $(this).remove();
            }
        };
    }
    else {
        close = options.close;
    }

    if (objectParams) {
        var params = [];
        for (var prop in objectParams) {
            params.push(prop.concat("=", objectParams[prop]));
        }
        url = url.concat("?", params.join("&"));
    }

    var dialog = $("<div></div>").dialog({
        modal: modal,
        autoOpen: autoOpen,
        resizable: resizable,
        width: width + 5,
        height: height + 50,
        minHeight: minHeight,
        minWidth: minWidth,
        maxHeight: maxHeight,
        maxWidth: maxWidth,
        title: titulo,
        close: close,
        buttons: options.buttons
    });

    if (iframe) {
        var frame = "<iframe src='{0}' width={1} height={2} ></iframe>".format(url, width, height);
        dialog.append(frame);
        log(frame);
        //var dialog = $(frame).dialog({
        //    modal: modal,
        //    autoOpen: autoOpen,
        //    resizable: resizable,
        //    width: width,
        //    height: height,
        //    minHeight: minHeight,
        //    minWidth: minWidth,
        //    maxHeight: maxHeight,
        //    maxWidth: maxWidth,
        //    title: titulo,
        //    close: close,
        //    buttons: options.buttons
        //});
    }
    else {


        dialog.load(url);
    }

    return dialog;
};

function mostrarModalByID(titulo, $ID, options, callBack) {
    //<summary>Carga una ventana modal con el Html Content recibido</summary>
    if (titulo == null) titulo = "Mensaje";

    if (options == null) options = new Object;
    if (callBack == null) callBack = function () { return false; }
    if (options.buttons == null) options.buttons =
    {
        Cerrar: function () {
            $(this).dialog("close");
        }
    };

    var width = options.width != null ? options.width : 'auto';
    var height = options.height != null ? options.height : 'auto';
    var autoOpen = options.autoOpen != null ? options.autoOpen : true;
    var resizable = options.resizable != null ? options.resizable : true;
    var modal = options.modal != null ? options.modal : true;
    var minHeight = options.minHeight != null ? options.minHeight : '180';//: '180',
    var minWidth = options.minWidth != null ? options.minWidth : '380';//: '380',
    var maxHeight = options.maxHeight != null ? options.maxHeight : '90%';//: '90%',
    var maxWidth = options.maxWidth != null ? options.maxWidth : '90%';//: '90%',

    var dialog = $ID.dialog({
        modal: modal,
        autoOpen: autoOpen,
        resizable: resizable,
        width: width,
        height: height,
        minHeight: minHeight,
        minWidth: minWidth,
        maxHeight: maxHeight,
        maxWidth: maxWidth,
        title: titulo,
        close: function (event, ui) {
            $(this).dialog("close");
            if (callBack != null) callBack();
            $(this).dialog("destroy");
        },
        buttons: options.buttons
    });

    return dialog;
};

var dateFormat = function () {
    var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
        timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
        timezoneClip = /[^-+\dA-Z]/g,
        pad = function (val, len) {
            val = String(val);
            len = len || 2;
            while (val.length < len) val = "0" + val;
            return val;
        };

    return function (date, mask, utc) {
        var dF = dateFormat;

        if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
            mask = date;
            date = undefined;
        }


        date = date ? new Date(date) : new Date;
        try {
            if (isNaN(date)) throw SyntaxError("invalid date");
        }
        catch (e) {

        }

        mask = String(dF.masks[mask] || mask || dF.masks["default"]);

        if (mask.slice(0, 4) == "UTC:") {
            mask = mask.slice(4);
            utc = true;
        }

        var _ = utc ? "getUTC" : "get",
            d = date[_ + "Date"](),
            D = date[_ + "Day"](),
            m = date[_ + "Month"](),
            y = date[_ + "FullYear"](),
            H = date[_ + "Hours"](),
            M = date[_ + "Minutes"](),
            s = date[_ + "Seconds"](),
            L = date[_ + "Milliseconds"](),
            o = utc ? 0 : date.getTimezoneOffset(),
            flags = {
                d: d,
                dd: pad(d),
                ddd: dF.i18n.dayNames[D],
                dddd: dF.i18n.dayNames[D + 7],
                m: m + 1,
                mm: pad(m + 1),
                mmm: dF.i18n.monthNames[m],
                mmmm: dF.i18n.monthNames[m + 12],
                yy: String(y).slice(2),
                yyyy: y,
                h: H % 12 || 12,
                hh: pad(H % 12 || 12),
                H: H,
                HH: pad(H),
                M: M,
                MM: pad(M),
                s: s,
                ss: pad(s),
                l: pad(L, 3),
                L: pad(L > 99 ? Math.round(L / 10) : L),
                t: H < 12 ? "a" : "p",
                tt: H < 12 ? "am" : "pm",
                T: H < 12 ? "A" : "P",
                TT: H < 12 ? "AM" : "PM",
                Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
            };

        return mask.replace(token, function ($0) {
            return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
        });
    };
}()

Date.prototype.format = function (mask, utc) {
    return dateFormat(this, mask, utc);
};

function formateaFechaJSON(Fecha, Formato) {
    if (vacio(Fecha)) { return ""; }
    var date = new Date(parseInt((Fecha.toString()).replace("/Date(", "").replace(")/", ""), 10));
    return dateFormat(date, Formato);
}

dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:MM:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:MM:ss",
    isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
};

dateFormat.i18n = {
    dayNames: [
        "Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab",
        "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sábado"
    ],
    monthNames: [
        "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic",
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
    ]
};

function isDate(txtDate) {
    var currVal = txtDate;
    if (currVal == '')
        return false;

    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //mm/dd/yyyy
    dtDay = dtArray[1];
    dtMonth = dtArray[3];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}

Number.prototype.formatMoney = function (c, d, t) {
    var n = this,
        c = isNaN(c = Math.abs(c)) ? 2 : c,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

function validInt(e, Text) {
    return keydownHandler(e);
}

function testLength(ta, maxLength) {
    if (ta.value.length > maxLength) {
        ta.value = ta.value.substring(0, maxLength);
    }
}

var mostrarConfirmar = function (titulo, mensaje, Observaciones, options, callBack) {
    if (mensaje == null) mensaje = "Descripción del Mensaje";
    if (titulo == null) titulo = "Mensaje";
    if (Observaciones == null) Observaciones = "";
    if (options == null) options = new Object;
    if (callBack == null) callBack = function () { return false; }

    var width = options.width != null ? options.width : 380;
    var height = options.height != null ? options.height : 180;
    var autoOpen = options.autoOpen != null ? options.autoOpen : true;
    var resizable = options.resizable != null ? options.resizable : false;
    var modal = options.modal != null ? options.modal : true;
    var maxLenObs = options.maxLenObs != null ? options.maxLenObs : 10;

    var txtObs = "";
    if (Observaciones.length > 0) {
        txtObs = '<br><textarea onchange="testLength(this,' + maxLenObs + ')" onkeyup="testLength(this,' + maxLenObs + ')" onpaste="testLength(this,' + maxLenObs + ')" cols="' + maxLenObs + '" maxlength="' + maxLenObs + '" style="resize: none;width:99%; margin:0;" id="' + Observaciones + '"></textarea>';
    }

    var dialog = $("<div><div id='lblMsgConfirmar' >" + mensaje + "</div>" + txtObs + "</div>").dialog({
        modal: true,
        autoOpen: autoOpen,
        resizable: resizable,
        width: width,
        height: height,
        title: titulo,
        open: function (event, ui) {
            var dz = $(".ui-dialog:last").css("z-index");
            $(".ui-widget-overlay:last").insertBefore(".ui-dialog:last");
            $(".ui-widget-overlay:last").css({ "z-index": dz - 1 });
        },
        close: function (event, ui) {
            $(this).remove();
        },
        buttons: {
            Aceptar: function () {
                callBack();
                $(this).dialog("close");
            },
            Cerrar: function () {
                $(this).dialog("close");
            }
        }
    });

    return dialog;
};

/*================================================================
                              Knockout
==================================================================*/

if (ko != undefined) {

    (function () {
        ko.debug = false;
        function toMoney(num) {
            if (num == null || num == undefined) { return "0.00"; }
            if (typeof num != Number) { num = Number(num); }
            return num.formatMoney();
            //return (Math.round(num * 100) / 100).toLocaleString();
        };

        function toDate(date) {
            if (date == null || date == undefined) { return ""; }
            if (typeof date != Date) { date = new Date(Date.parse(date)); }
            return date.toIsoDate();
        }

        ko.bindingHandlers.money = {
            update: function (element, valueAccessor, allBindings) {
                if (element.value != undefined) {
                    element.value = toMoney(ko.unwrap(valueAccessor()))
                }
                else {
                    element.innerHTML = toMoney(ko.unwrap(valueAccessor()));
                }
            }
        };
        ko.bindingHandlers.date = {
            update: function (element, valueAccessor, allBindings) {
                if (element.value != undefined) {
                    element.value = toDate(ko.unwrap(valueAccessor()))
                }
                else {
                    element.innerHTML = toDate(ko.unwrap(valueAccessor()));
                }
            }
        };
        ko.bindingHandlers.readonly = {
            update: function (element, valueAccessor) {
                if (!ko.unwrap(valueAccessor())) {
                    element.setAttribute("readonly", true);
                } else {
                    element.removeAttribute("readonly");
                }
            }
        };
        ko.bindingHandlers.hidden = {
            update: function (element, valueAccessor) {
                if (ko.unwrap(valueAccessor())) {
                    element.setAttribute("style", "display:none;");
                } else {
                    element.removeAttribute("style");
                }
            }
        };
        ko.bindingHandlers.nullchecked = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var value = ko.unwrap(valueAccessor());
                if (value == null) {
                    element.indeterminate = true;
                }
                else {
                    element.indeterminate = false;
                    element.checked = value;
                }
            }
        };

        ko.bindingHandlers.numeric = {
            init: function (element, valueAccessor) {
                $(element).forceNumeric();
            }
        };
        ko.bindingHandlers.smartnumeric = {
            init: function (element, valueAccessor) {
                $(element).aplicarMoneda();
            }
        };
        //ko.bindingHandlers.onchange = {
        //    init: function (element, valueAccessor, allBindings, viewModel, bindingContext)
        //    {
        //        valueAccessor
        //        // This will be called when the binding is first applied to an element
        //        // Set up any initial state, event handlers, etc. here
        //    },
        //    update: function (element, valueAccessor, allBindings, viewModel, bindingContext)
        //    {
        //        var evento = ko.unwrap(valueAccessor());
        //        evento(element);
        //        // This will be called once when the binding is first applied to an element,
        //        // and again whenever any observables/computeds that are accessed change
        //        // Update the DOM element based on the supplied values here.
        //    }
        //};
    })();

    ko.extenders.numeric = function (target, precision) {
        //console.info("numeric");
        //create a writable computed observable to intercept writes to our observable
        var result = ko.pureComputed({
            read: target,  //always return the original observables value
            write: function (newValue) {
                //console.info("numeric write");
                var current = target(),
                    roundingMultiplier = Math.pow(10, precision),
                    newValueAsNum = isNaN(newValue) ? 0 : parseFloat(+newValue),
                    valueToWrite = Math.round(newValueAsNum * roundingMultiplier) / roundingMultiplier;

                //only write if it changed
                if (valueToWrite !== current) {
                    target(valueToWrite);
                } else {
                    //if the rounded value is the same, but a different value was written, force a notification for the current field
                    if (newValue !== current) {
                        target.notifySubscribers(valueToWrite);
                    }
                }
            }
        }).extend({ notify: 'always' });

        //initialize with current value to make sure it is rounded appropriately
        result(target());

        //return the new computed observable
        return result;
    };

    ko.extenders.required = function (target, overrideMessage) {
        //console.info("required");
        //add some sub-observables to our observable
        target.hasError = ko.observable();
        target.validationMessage = ko.observable();

        //define a function to do validation
        function validate(newValue) {
            //console.info("required validate");
            target.hasError(newValue ? false : true);
            target.validationMessage(newValue ? "" : overrideMessage || "This field is required");
        }

        //initial validation
        validate(target());

        //validate whenever the value changes
        target.subscribe(validate);

        //return the original observable
        return target;
    };

    ko.unapplyBindings = function ($node, remove) {
        // unbind events
        $node.find("*").each(function () {
            $(this).unbind();
        });

        // Remove KO subscriptions and references
        if (remove) {
            ko.removeNode($node[0]);
        } else {
            ko.cleanNode($node[0]);
        }
    };
}

/*================================================================
                              Underscore
==================================================================*/
/* Ordena el colection en base al iteratee. 
selfsort en true indica que debe ordenar el colection actual en lugar de crear una copia
caseful en true indica que las mayúsculas se colocan antes que las minúsculas
*/
_.sortBy = function (colection, iteratee, context, selfsort, caseful) {
    iteratee = _.iteratee(iteratee, context);

    var at, bt;
    function orden(a, b) {
        at = Number(iteratee(a));
        bt = Number(iteratee(b));

        if (!_.isNaN(at) && !_.isNaN(bt)) {
            return at - bt;
        }

        at = iteratee(a);
        bt = iteratee(b);

        if (caseful == undefined || caseful == false) {

            if (typeof (at) == typeof ("")) {
                at = at.toLowerCase();
            }
            if (typeof (bt) == typeof ("")) {
                bt = bt.toLowerCase();
            }
        }

        if (at !== bt) {
            if (at > bt || at === void 0) return 1;
            if (at < bt || bt === void 0) return -1;
        }
        return 0;
    }
    if (!_.isUndefined(selfsort) && selfsort == true) {
        return colection.sort(orden);
    }
    return colection.slice().sort(orden);
};
/* Suma los valores del iteratee especificado */
_.sum = function (colection, iteratee) {
    iteratee = _.iteratee(iteratee, context);
    var sum = 0;
    for (var i = 0, length = collection.length; i < length; i++) {
        sum += iteratee(collection[i]);
    }
    return sum;
}
