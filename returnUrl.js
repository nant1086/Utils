function reautenticate() {
		location.href = '/LoginController/Login?ReturnUrl=' + encodeURIComponent(location.href.substring(location.origin.length));
}