var loadedscripts = document.getElementsByTagName("script");
var helpdeskUrl = "http://helpdesk.agapeconnect.me/" // loadedscripts[loadedscripts.length - 1].src;
//helpdeskUrl = helpdeskUrl.split('/').slice(0, -1).join('/');
//helpdeskUrl = helpdeskUrl.split('/').slice(0, -1).join('/') + '/';

var fancyboxloaded = false;

LoadSupportWidget();

function LoadSupportWidget() {
	if(helpdeskUrl.substr(helpdeskUrl.length-1)!="/") //normalizing the URL
		helpdeskUrl += "/";

	//add css style
	AddFancyBoxCSS();

	//load jquery
	//GetScript("https://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js");
	TryJQueryReady(0);
}

function CreateSupportWidget(widgetalign) {
	var xPos = (widgetalign == "right") ? "right:0px" : "left:0px";

	//add widget
	document.write("<a class='SupportWidget' style='display:block;position:fixed;top:50%;margin-top:-64px;" + xPos + "'>");
	document.write("<img src='" + helpdeskUrl + "images/support_tab.png' style='border:none;' />");
	document.write("</a>");

	TryJQueryReady(0);
}

//this function is for backward compatibility only
function SupportWidget(hdUrl, widgetalign) {
	if(widgetalign)
		CreateSupportWidget(widgetalign);
}

function SupportWidgetStep2() {
	$(document).ready(function () {
		$('.SupportWidget').attr('href', helpdeskUrl + 'Tickets/New/?SimpleMaster=1');

		if (!fancyboxloaded) { //to prevent adding fancybox 2 times
		    fancyboxloaded = true;
		    GetScript("/js/jquery.fancybox-1.3.4.pack.js");
			//GetScript("https://jitbitscripts.googlecode.com/svn/trunk/jquery.fancybox-1.3.4.pack.js");
		}
		TryFancyboxReady(0);
	});
}

function SupportWidgetStep3() {
	//not for iphones and ipads
	if (navigator.userAgent.match(/iPhone/i) || navigator.userAgent.match(/iPod/i) || navigator.userAgent.match(/iPad/i)) return;

	$('.SupportWidget').fancybox({
		'width': 800,
		'height': 550,
		'autoScale': true,
		'transitionIn': 'fade',
		'transitionOut': 'none',
		'type': 'iframe',
		padding: 0
	});
	$.fancybox.init(); //reinit - safari fix
}

function AddFancyBoxCSS() {
	var headtg = document.getElementsByTagName('head')[0];
	if (!headtg) {
		return;
	}
	var linktg = document.createElement('link');
	linktg.type = 'text/css';
	linktg.rel = 'stylesheet';
	linktg.href = '/js/jquery.fancybox-1.3.4.css';
	headtg.appendChild(linktg);
}

// dynamically load any javascript file.
function GetScript(filename) {
	var script = document.createElement('script')
	script.setAttribute("type", "text/javascript")
	script.setAttribute("src", filename)
	if (typeof script != "undefined")
		document.getElementsByTagName("head")[0].appendChild(script)
}

function TryJQueryReady(time_elapsed) {
	// Continually polls to see if jQuery is loaded.
	if (typeof $ == "undefined") { // if jQuery isn't loaded yet...
		if (time_elapsed <= 5000) { // and we havn't given up trying...
			setTimeout("TryJQueryReady(" + (time_elapsed + 200) + ")", 200); // set a timer to check again in 200 ms.
		} else {
			alert("Timed out while loading jQuery.")
		}
	} else {
		SupportWidgetStep2();
	}
}

function TryFancyboxReady(time_elapsed) {
	// Continually polls to see if fancybox is loaded.
	if (typeof $.fancybox == "undefined") { // if fancybox isn't loaded yet...
		if (time_elapsed <= 5000) { // and we havn't given up trying...
			setTimeout("TryFancyboxReady(" + (time_elapsed + 200) + ")", 200); // set a timer to check again in 200 ms.
		} else {
			alert("Timed out while loading fancybox-script...")
		}
	} else {
		SupportWidgetStep3();
	}
}