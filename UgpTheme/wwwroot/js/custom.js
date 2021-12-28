function _serializeForm(id) {
	var result = {};
	$.each($(id).serializeArray(), function (i, field) {
		result[field.name] = field.value.trim() || null;
	});
	return result;
}

function interceptLogin(formId, userInputId, passInputId) {
	$("#" + formId).submit(function (e) {
		e.preventDefault();
		let frmData = _serializeForm("#" + formId);
		let data = {
			userName: frmData[userInputId],
			password: frmData[passInputId]
		}
		var $form = $(this);
		$.ajax({
			type: "POST",
			url: "/api/LegacyAccount",
			data: JSON.stringify(data),
			contentType: "application/json; charset=utf-8",
			context: $form,
			success: function (usr) {
				$('#' + userInputId).val(usr);
				this.off('submit');
				this.submit();
			},
			error: function (err) {
				if (err.status === 400 && err.responseJSON && err.responseJSON.detail) {
					$("#" + formId + " .validation-summary-valid").html(err.responseJSON.detail)
				}
				console.log(err);
			}
		});
	});
}