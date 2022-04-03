var datatable;


$(document).ready(function () {
    loadDatatable();
})

/*#TODO solve isverified*/
function loadDatatable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url":"/AdminPanel/VerifiedUser/GetAll"
        },
        "columns": [
            {
                "data":"userName","Width":"20%"
            },
            {
                "data": "email", "Width": "20%"
            },
            
            {
                "data": "phoneNumber", "Width": "20%"
            },
            {
                "data": "isVerified",
                "render": function (data) {
                    if (data) {
                        return `<input type="checkbox" disabled checked />`
                    }
                    else {
                        return `<input type="checkbox" disabled />`
                    }
                }
            },
            {
                "data": "email",
                "render": function (data) {
                    return `
                    <div class="text-center">
                    <a href="/AdminPanel/VerifiedUser/upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                    <i class="fas fa-edit"></i>
                    </a>
                    <a  onclick=Delete("/AdminPanel/VerifiedUser/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                    <i class="fas fa-trash-alt"></i>
                    </a>
                    </div>`
                },
                "width":"30%"
            }
        ]
    })
}


function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode:true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax(
                {
                    type: "DELETE",
                    url: url,
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message);
                            dataTable.ajax.reload();
                        } else {
                            toastr.error(data.message);
                        }
                    }
                })
        }
        
    })
}