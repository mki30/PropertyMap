(function (U, V)
{
    var i, N, O, P, Q, s, x, G, z, R, H, C, u, I, v, D, J, K, S, L, w, E, M, F; i = function (a) { return new i.prototype.init(a) }; typeof require !== "undefined" && typeof exports !== "undefined" && typeof module !== "undefined" ? module.exports = i : U.Globalize = i; i.cultures = {}; i.prototype = { constructor: i, init: function (a) { this.cultures = i.cultures; this.cultureSelector = a; return this } }; i.prototype.init.prototype = i.prototype; i.cultures["default"] = {
        name: "en", englishName: "English", nativeName: "English", isRTL: !1, language: "en", numberFormat: {
            pattern: ["-n"],
            decimals: 0, ",": ",", ".": ".", groupSizes: [3], "+": "+", "-": "-", NaN: "NaN", negativeInfinity: "-Infinity", positiveInfinity: "Infinity", percent: { pattern: ["-n %", "n %"], decimals: 2, groupSizes: [3], ",": ",", ".": ".", symbol: "%" }, currency: { pattern: ["($n)", "$n"], decimals: 2, groupSizes: [3], ",": ",", ".": ".", symbol: "$" }
        }, calendars: {
            standard: {
                name: "Gregorian_USEnglish", "/": "/", ":": ":", firstDay: 0, days: {
                    names: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"], namesAbbr: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri",
                    "Sat"], namesShort: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"]
                }, months: { names: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", ""], namesAbbr: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", ""] }, AM: ["AM", "am", "AM"], PM: ["PM", "pm", "PM"], eras: [{ name: "A.D.", start: null, offset: 0 }], twoDigitYearMax: 2029, patterns: {
                    d: "M/d/yyyy", D: "dddd, MMMM dd, yyyy", t: "h:mm tt", T: "h:mm:ss tt", f: "dddd, MMMM dd, yyyy h:mm tt", F: "dddd, MMMM dd, yyyy h:mm:ss tt",
                    M: "MMMM dd", Y: "yyyy MMMM", S: "yyyy'-'MM'-'dd'T'HH':'mm':'ss"
                }
            }
        }, messages: {}
    }; i.cultures["default"].calendar = i.cultures["default"].calendars.standard; i.cultures.en = i.cultures["default"]; i.cultureSelector = "en"; N = /^0x[a-f0-9]+$/i; O = /^[+-]?infinity$/i; P = /^[+-]?\d*\.?\d*(e[+-]?\d+)?$/; Q = /^\s+|\s+$/g; s = function (a, b) { if (a.indexOf) return a.indexOf(b); for (var c = 0, d = a.length; c < d; c++) if (a[c] === b) return c; return -1 }; x = function (a, b) { return a.substr(a.length - b.length) === b }; G = function (a)
    {
        var b, c, d, e, g, h = arguments[0] ||
        {}, f = 1, j = arguments.length, a = !1; typeof h === "boolean" && (a = h, h = arguments[1] || {}, f = 2); for (typeof h !== "object" && !R(h) && (h = {}) ; f < j; f++) if ((b = arguments[f]) != null) for (c in b) d = h[c], e = b[c], h !== e && (a && e && (H(e) || (g = z(e))) ? (g ? (g = !1, d = d && z(d) ? d : []) : d = d && H(d) ? d : {}, h[c] = G(a, d, e)) : e !== V && (h[c] = e)); return h
    }; z = Array.isArray || function (a) { return Object.prototype.toString.call(a) === "[object Array]" }; R = function (a) { return Object.prototype.toString.call(a) === "[object Function]" }; H = function (a)
    {
        return Object.prototype.toString.call(a) ===
        "[object Object]"
    }; C = function (a, b) { return a.indexOf(b) === 0 }; u = function (a) { return (a + "").replace(Q, "") }; I = function (a) { return a | 0 }; v = function (a, b, c) { var d; for (d = a.length; d < b; d += 1) a = c ? "0" + a : a + "0"; return a }; D = function (a, b) { for (var c = 0, d = !1, e = 0, g = a.length; e < g; e++) { var h = a.charAt(e); switch (h) { case "'": d ? b.push("'") : c++; d = !1; break; case "\\": d && b.push("\\"); d = !d; break; default: b.push(h), d = !1 } } return c }; J = function (a, b)
    {
        var b = b || "F", c; c = a.patterns; var d = b.length; if (d === 1)
        {
            c = c[b]; if (!c) throw "Invalid date format string '" +
            b + "'."; b = c
        } else d === 2 && b.charAt(0) === "%" && (b = b.charAt(1)); return b
    }; K = function (a, b, c)
    {
        function d(a, b) { var f; f = a + ""; if (b > 1 && f.length < b) return f = i[b - 2] + f, f.substr(f.length - b, b); return f } function e() { if (q || r) return q; q = A.test(b); r = !0; return q } function g(a, b) { if (l) return l[b]; switch (b) { case 0: return a.getFullYear(); case 1: return a.getMonth(); case 2: return a.getDate() } } var h = c.calendar, f = h.convert; if (!b || !b.length || b === "i")
        {
            if (c && c.name.length) if (f) c = K(a, h.patterns.F, c); else
            {
                var c = new Date(a.getTime()),
                j = w(a, h.eras); c.setFullYear(E(a, h, j)); c = c.toLocaleString()
            } else c = a.toString(); return c
        } var j = h.eras, T = b === "s", b = J(h, b), c = [], k, i = ["0", "00", "000"], q, r, A = /([^d]|^)(d|dd)([^d]|$)/g, o = 0, m = L(), l; for (!T && f && (l = f.fromGregorian(a)) ; ;)
        {
            k = m.lastIndex; f = m.exec(b); k = b.slice(k, f ? f.index : b.length); o += D(k, c); if (!f) break; if (o % 2) c.push(f[0]); else switch (k = f[0], f = k.length, k)
            {
                case "ddd": case "dddd": c.push((f === 3 ? h.days.namesAbbr : h.days.names)[a.getDay()]); break; case "d": case "dd": q = !0; c.push(d(g(a, 2), f)); break; case "MMM": case "MMMM": k =
                g(a, 1); c.push(h.monthsGenitive && e() ? h.monthsGenitive[f === 3 ? "namesAbbr" : "names"][k] : h.months[f === 3 ? "namesAbbr" : "names"][k]); break; case "M": case "MM": c.push(d(g(a, 1) + 1, f)); break; case "y": case "yy": case "yyyy": k = l ? l[0] : E(a, h, w(a, j), T); f < 4 && (k %= 100); c.push(d(k, f)); break; case "h": case "hh": k = a.getHours() % 12; k === 0 && (k = 12); c.push(d(k, f)); break; case "H": case "HH": c.push(d(a.getHours(), f)); break; case "m": case "mm": c.push(d(a.getMinutes(), f)); break; case "s": case "ss": c.push(d(a.getSeconds(), f)); break; case "t": case "tt": k =
                a.getHours() < 12 ? h.AM ? h.AM[0] : " " : h.PM ? h.PM[0] : " "; c.push(f === 1 ? k.charAt(0) : k); break; case "f": case "ff": case "fff": c.push(d(a.getMilliseconds(), 3).substr(0, f)); break; case "z": case "zz": k = a.getTimezoneOffset() / 60; c.push((k <= 0 ? "+" : "-") + d(Math.floor(Math.abs(k)), f)); break; case "zzz": k = a.getTimezoneOffset() / 60; c.push((k <= 0 ? "+" : "-") + d(Math.floor(Math.abs(k)), 2) + ":" + d(Math.abs(a.getTimezoneOffset() % 60), 2)); break; case "g": case "gg": h.eras && c.push(h.eras[w(a, j)].name); break; case "/": c.push(h["/"]); break; default: throw "Invalid date format pattern '" +
                k + "'.";
            }
        } return c.join("")
    }; (function ()
    {
        var a; a = function (a, c, d)
        {
            var e = d.groupSizes, g = e[0], h = 1, f = Math.pow(10, c), j = Math.round(a * f) / f; isFinite(j) || (j = a); f = ""; f = (j + "").split(/e/i); j = f.length > 1 ? parseInt(f[1], 10) : 0; a = f[0]; f = a.split("."); a = f[0]; f = f.length > 1 ? f[1] : ""; j > 0 ? (f = v(f, j, !1), a += f.slice(0, j), f = f.substr(j)) : j < 0 && (j = -j, a = v(a, j + 1), f = a.slice(-j, a.length) + f, a = a.slice(0, -j)); f = c > 0 ? d["."] + (f.length > c ? f.slice(0, c) : v(f, c)) : ""; c = a.length - 1; d = d[","]; for (j = ""; c >= 0;)
            {
                if (g === 0 || g > c) return a.slice(0, c + 1) + (j.length ?
                d + j + f : f); j = a.slice(c - g + 1, c + 1) + (j.length ? d + j : ""); c -= g; h < e.length && (g = e[h], h++)
            } return a.slice(0, c + 1) + d + j + f
        }; S = function (b, c, d)
        {
            if (!isFinite(b)) { if (b === Infinity) return d.numberFormat.positiveInfinity; if (b === -Infinity) return d.numberFormat.negativeInfinity; return d.numberFormat.NaN } if (!c || c === "i") return d.name.length ? b.toLocaleString() : b.toString(); var c = c || "D", d = d.numberFormat, e = Math.abs(b), g = -1; c.length > 1 && (g = parseInt(c.slice(1), 10)); var h = c.charAt(0).toUpperCase(), f; switch (h)
            {
                case "D": c = "n"; e = I(e);
                    g !== -1 && (e = v("" + e, g, !0)); b < 0 && (e = "-" + e); break; case "N": f = d; case "C": f = f || d.currency; case "P": f = f || d.percent; c = b < 0 ? f.pattern[0] : f.pattern[1] || "n"; if (g === -1) g = f.decimals; e = a(e * (h === "P" ? 100 : 1), g, f); break; default: throw "Bad number format specifier: " + h;
            } b = /n|\$|-|%/g; for (f = ""; ;) { g = b.lastIndex; h = b.exec(c); f += c.slice(g, h ? h.index : c.length); if (!h) break; switch (h[0]) { case "n": f += e; break; case "$": f += d.currency.symbol; break; case "-": /[1-9]/.test(e) && (f += d["-"]); break; case "%": f += d.percent.symbol } } return f
        }
    })();
    L = function () { return /\/|dddd|ddd|dd|d|MMMM|MMM|MM|M|yyyy|yy|y|hh|h|HH|H|mm|m|ss|s|tt|t|fff|ff|f|zzz|zz|z|gg|g/g }; w = function (a, b) { if (!b) return 0; for (var c, d = a.getTime(), e = 0, g = b.length; e < g; e++) if (c = b[e].start, c === null || d >= c) return e; return 0 }; E = function (a, b, c, d) { a = a.getFullYear(); !d && b.eras && (a -= b.eras[c].offset); return a }; (function ()
    {
        var a, b, c, d, e, g, h; a = function (a, b)
        {
            var c = new Date, d = w(c); if (b < 100)
            {
                var e = a.twoDigitYearMax, e = typeof e === "string" ? (new Date).getFullYear() % 100 + parseInt(e, 10) : e, c = E(c, a, d);
                b += c - c % 100; b > e && (b -= 100)
            } return b
        }; b = function (a, b, c) { var d = a.days, e = a._upperDays; if (!e) a._upperDays = e = [h(d.names), h(d.namesAbbr), h(d.namesShort)]; b = g(b); c ? (a = s(e[1], b), a === -1 && (a = s(e[2], b))) : a = s(e[0], b); return a }; c = function (a, b, c) { var d = a.months, e = a.monthsGenitive || a.months, i = a._upperMonths, r = a._upperMonthsGen; if (!i) a._upperMonths = i = [h(d.names), h(d.namesAbbr)], a._upperMonthsGen = r = [h(e.names), h(e.namesAbbr)]; b = g(b); a = s(c ? i[1] : i[0], b); a < 0 && (a = s(c ? r[1] : r[0], b)); return a }; d = function (a, b)
        {
            var c = a._parseRegExp;
            if (c) { var d = c[b]; if (d) return d } else a._parseRegExp = c = {}; for (var d = J(a, b).replace(/([\^\$\.\*\+\?\|\[\]\(\)\{\}])/g, "\\\\$1"), e = ["^"], h = [], g = 0, i = 0, o = L(), m; (m = o.exec(d)) !== null;)
            {
                var l = d.slice(g, m.index), g = o.lastIndex; i += D(l, e); if (i % 2) e.push(m[0]); else
                {
                    var l = m[0], u = l.length; switch (l)
                    {
                        case "dddd": case "ddd": case "MMMM": case "MMM": case "gg": case "g": l = "(\\D+)"; break; case "tt": case "t": l = "(\\D*)"; break; case "yyyy": case "fff": case "ff": case "f": l = "(\\d{" + u + "})"; break; case "dd": case "d": case "MM": case "M": case "yy": case "y": case "HH": case "H": case "hh": case "h": case "mm": case "m": case "ss": case "s": l =
                        "(\\d\\d?)"; break; case "zzz": l = "([+-]?\\d\\d?:\\d{2})"; break; case "zz": case "z": l = "([+-]?\\d\\d?)"; break; case "/": l = "(\\" + a["/"] + ")"; break; default: throw "Invalid date format pattern '" + l + "'.";
                    } l && e.push(l); h.push(m[0])
                }
            } D(d.slice(g), e); e.push("$"); d = { regExp: e.join("").replace(/\s+/g, "\\s+"), groups: h }; return c[b] = d
        }; e = function (a, b, c) { return a < b || a > c }; g = function (a) { return a.split("\u00a0").join(" ").toUpperCase() }; h = function (a) { for (var b = [], c = 0, d = a.length; c < d; c++) b[c] = g(a[c]); return b }; M = function (f,
        h, g)
        {
            var f = u(f), g = g.calendar, h = d(g, h), i = RegExp(h.regExp).exec(f); if (i === null) return null; var s = h.groups, q = h = f = null, r = null, A = null, o = 0, m, l = 0, v = 0, w = 0; m = null; for (var x = !1, y = 0, z = s.length; y < z; y++)
            {
                var n = i[y + 1]; if (n)
                {
                    var p = s[y], B = p.length, t = parseInt(n, 10); switch (p)
                    {
                        case "dd": case "d": r = t; if (e(r, 1, 31)) return null; break; case "MMM": case "MMMM": q = c(g, n, B === 3); if (e(q, 0, 11)) return null; break; case "M": case "MM": q = t - 1; if (e(q, 0, 11)) return null; break; case "y": case "yy": case "yyyy": h = B < 4 ? a(g, t) : t; if (e(h, 0, 9999)) return null;
                            break; case "h": case "hh": o = t; o === 12 && (o = 0); if (e(o, 0, 11)) return null; break; case "H": case "HH": o = t; if (e(o, 0, 23)) return null; break; case "m": case "mm": l = t; if (e(l, 0, 59)) return null; break; case "s": case "ss": v = t; if (e(v, 0, 59)) return null; break; case "tt": case "t": x = g.PM && (n === g.PM[0] || n === g.PM[1] || n === g.PM[2]); if (!x && (!g.AM || n !== g.AM[0] && n !== g.AM[1] && n !== g.AM[2])) return null; break; case "f": case "ff": case "fff": w = t * Math.pow(10, 3 - B); if (e(w, 0, 999)) return null; break; case "ddd": case "dddd": A = b(g, n, B === 3); if (e(A,
                            0, 6)) return null; break; case "zzz": p = n.split(/:/); if (p.length !== 2) return null; m = parseInt(p[0], 10); if (e(m, -12, 13)) return null; p = parseInt(p[1], 10); if (e(p, 0, 59)) return null; m = m * 60 + (C(n, "-") ? -p : p); break; case "z": case "zz": m = t; if (e(m, -12, 13)) return null; m *= 60; break; case "g": case "gg": if (!n || !g.eras) return null; n = u(n.toLowerCase()); p = 0; for (B = g.eras.length; p < B; p++) if (n === g.eras[p].name.toLowerCase()) { f = p; break } if (f === null) return null
                    }
                }
            } i = new Date; s = (y = g.convert) ? y.fromGregorian(i)[0] : i.getFullYear(); h ===
            null ? h = s : g.eras && (h += g.eras[f || 0].offset); q === null && (q = 0); r === null && (r = 1); if (y) { if (i = y.toGregorian(h, q, r), i === null) return null } else { i.setFullYear(h, q, r); if (i.getDate() !== r) return null; if (A !== null && i.getDay() !== A) return null } x && o < 12 && (o += 12); i.setHours(o, l, v, w); m !== null && (g = i.getMinutes() - (m + i.getTimezoneOffset()), i.setHours(i.getHours() + parseInt(g / 60, 10), g % 60)); return i
        }
    })(); F = function (a, b, c)
    {
        var d = b["-"], b = b["+"], e; switch (c)
        {
            case "n -": d = " " + d, b = " " + b; case "n-": x(a, d) ? e = ["-", a.substr(0, a.length -
            d.length)] : x(a, b) && (e = ["+", a.substr(0, a.length - b.length)]); break; case "- n": d += " ", b += " "; case "-n": C(a, d) ? e = ["-", a.substr(d.length)] : C(a, b) && (e = ["+", a.substr(b.length)]); break; case "(n)": C(a, "(") && x(a, ")") && (e = ["-", a.substr(1, a.length - 2)])
        } return e || ["", a]
    }; i.prototype.findClosestCulture = function (a) { return i.findClosestCulture.call(this, a) }; i.prototype.format = function (a, b, c) { return i.format.call(this, a, b, c) }; i.prototype.localize = function (a, b) { return i.localize.call(this, a, b) }; i.prototype.parseInt =
    function (a, b, c) { return i.parseInt.call(this, a, b, c) }; i.prototype.parseFloat = function (a, b, c) { return i.parseFloat.call(this, a, b, c) }; i.prototype.culture = function (a) { return i.culture.call(this, a) }; i.addCultureInfo = function (a, b, c) { var d = {}, e = !1; typeof a !== "string" ? (c = a, a = this.culture().name, d = this.cultures[a]) : typeof b !== "string" ? (c = b, e = this.cultures[a] == null, d = this.cultures[a] || this.cultures["default"]) : (e = !0, d = this.cultures[b]); this.cultures[a] = G(!0, {}, d, c); if (e) this.cultures[a].calendar = this.cultures[a].calendars.standard };
    i.findClosestCulture = function (a)
    {
        var b; if (!a) return this.cultures[this.cultureSelector] || this.cultures["default"]; typeof a === "string" && (a = a.split(",")); if (z(a))
        {
            var c, d = this.cultures, e = a, g, h = e.length, f = []; for (g = 0; g < h; g++) a = u(e[g]), a = a.split(";"), c = u(a[0]), a.length === 1 ? a = 1 : (a = u(a[1]), a.indexOf("q=") === 0 ? (a = a.substr(2), a = parseFloat(a), a = isNaN(a) ? 0 : a) : a = 1), f.push({ lang: c, pri: a }); f.sort(function (a, b) { return a.pri < b.pri ? 1 : -1 }); for (g = 0; g < h; g++) if (c = f[g].lang, b = d[c]) return b; for (g = 0; g < h; g++)
            {
                c = f[g].lang;
                do { e = c.lastIndexOf("-"); if (e === -1) break; c = c.substr(0, e); if (b = d[c]) return b } while (1)
            } for (g = 0; g < h; g++) { c = f[g].lang; for (var i in d) if (e = d[i], e.language == c) return e }
        } else if (typeof a === "object") return a; return b || null
    }; i.format = function (a, b, c) { culture = this.findClosestCulture(c); a instanceof Date ? a = K(a, b, culture) : typeof a === "number" && (a = S(a, b, culture)); return a }; i.localize = function (a, b) { return this.findClosestCulture(b).messages[a] || this.cultures["default"].messages.key }; i.parseDate = function (a, b, c)
    {
        var c =
        this.findClosestCulture(c), d, e; if (b) { if (typeof b === "string" && (b = [b]), b.length) { e = 0; for (var g = b.length; e < g; e++) { var h = b[e]; if (h && (d = M(a, h, c))) break } } } else for (e in b = c.calendar.patterns, b) if (d = M(a, b[e], c)) break; return d || null
    }; i.parseInt = function (a, b, c) { return I(i.parseFloat(a, b, c)) }; i.parseFloat = function (a, b, c)
    {
        typeof b !== "number" && (c = b, b = 10); var d = this.findClosestCulture(c), c = NaN, e = d.numberFormat; a.indexOf(d.numberFormat.currency.symbol) > -1 && (a = a.replace(d.numberFormat.currency.symbol, ""), a = a.replace(d.numberFormat.currency["."],
        d.numberFormat["."])); a = u(a); if (O.test(a)) c = parseFloat(a); else if (!b && N.test(a)) c = parseInt(a, 16); else
        {
            d = F(a, e, e.pattern[0]); b = d[0]; d = d[1]; b === "" && e.pattern[0] !== "-n" && (d = F(a, e, "-n"), b = d[0], d = d[1]); var b = b || "+", g, a = d.indexOf("e"); a < 0 && (a = d.indexOf("E")); a < 0 ? (g = d, a = null) : (g = d.substr(0, a), a = d.substr(a + 1)); var h = e["."], f = g.indexOf(h); f < 0 ? (d = g, g = null) : (d = g.substr(0, f), g = g.substr(f + h.length)); h = e[","]; d = d.split(h).join(""); f = h.replace(/\u00A0/g, " "); h !== f && (d = d.split(f).join("")); b += d; g !== null && (b += "." +
            g); a !== null && (e = F(a, e, "-n"), b += "e" + (e[0] || "+") + e[1]); P.test(b) && (c = parseFloat(b))
        } return c
    }; i.culture = function (a) { if (typeof a !== "undefined") this.cultureSelector = a; return this.findClosestCulture(a) || this.culture["default"] }
})(this);