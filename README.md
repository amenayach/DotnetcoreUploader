# DotnetcoreUploader
Simple demo Web API showing how to upload single or multiple files using Dotnetcore

## How to upload via jQuery
```js
var dataForm = new FormData();
dataForm.append("file1OrWhatEver", document.getElementById('fileuploadDom1').files[0]);
dataForm.append("infos",JSON.stringify({Id: 1, Name: "testname"}));

$.ajax({
	url: 'http://localhost:63798/api/Up',
	type: 'POST', 
	contentType: false,
	processData: false,
	data: dataForm,
	success: function (data) {
		console.log("Done");
	},
	error: function (jqXHR, textStatus, errorThrown) {
		console.log(jqXHR);
	},
	xhr: function(){     
		var xhr = $.ajaxSettings.xhr() ;     
		xhr.upload.onprogress = function(evt){ 
			// logging percentage
			console.log(evt.loaded/evt.total);
		};
		xhr.upload.onload = function(){ /*console.log('DONE!');*/ } ;     
		return xhr ;
	}
});
```
