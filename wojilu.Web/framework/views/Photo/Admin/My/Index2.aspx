	<link href="~static/flash/swfupload/default.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="~static/flash/swfupload/swfupload.js"></script>
	<script type="text/javascript" src="~static/flash/swfupload/handlers.js"></script>
	<script type="text/javascript">
		var swfu;
		window.onload = function () {
			swfu = new SWFUpload({
				// Backend Settings
				upload_url: "#{uploadLink}",
                //post_params : {
                //    "ASPSESSID" : "<%=Session.SessionID %>"
                //},

				// File Upload Settings
				file_size_limit : "800 MB",
				file_types : "*.*",
				file_types_description : "所有文件",
				file_upload_limit : "0",    // Zero means unlimited

				// Event Handler Settings - these functions as defined in Handlers.js
				//  The handlers are not part of SWFUpload but are part of my website and control how
				//  my website reacts to the SWFUpload events.
				file_queue_error_handler : fileQueueError,
				file_dialog_complete_handler : fileDialogComplete,
				upload_progress_handler : uploadProgress,
				upload_error_handler : uploadError,
				upload_success_handler : uploadSuccess,
				upload_complete_handler : uploadComplete,

				// Button settings
				button_image_url : "~static/flash/swfupload/XPButtonNoText_160x22.png",
				button_placeholder_id : "spanButtonPlaceholder",
				button_width: 160,
				button_height: 22,
				button_text : '<span class="button">请选择图片<span class="buttonSmall">(200 MB Max)</span></span>',
				button_text_style : '.button { font-family: Helvetica, Arial, sans-serif; font-size: 14pt; } .buttonSmall { font-size: 10pt; }',
				button_text_top_padding: 1,
				button_text_left_padding: 5,

				// Flash Settings
				flash_url : "/static/flash/swfupload/swfupload.swf",	// Relative to this file

				custom_settings : {
					upload_target : "divFileProgressContainer"
				},

				// Debug Settings
				debug: true
			});
		}
	</script>


<h1 style="background-color:ButtonShadow;">SWFUpload v2.2.0</h1>	
<div id="content">
    
	<h2>Application Demo (ASP.Net 2.0)</h2>
	
    <div id="swfu_container" style="margin: 0px 10px;">
	    <div>
			<span id="spanButtonPlaceholder"></span>
	    </div>
	    <div id="divFileProgressContainer" style="height: 75px;"></div>
	    <div id="thumbnails"></div>
    </div>
    
</div>