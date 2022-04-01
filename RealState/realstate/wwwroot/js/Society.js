var datatable;


$(document).ready(function () {
    loadDatatable();
})

function loadDatatable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url":"/AdminPanel/society/GetAll"
        },
        "columns": [
            {
                "data":"name","Width":"20%"
            },
            {
                "data": "locationTBL.city", "Width": "20%"
            },
            {
                "data": "societyId",
                "render": function (data) {
                    

                    return `
                    <div class="text-center">
                    <a href="/AdminPanel/society/upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                    <i class="fas fa-edit"></i>
                    </a>
                    <a  onclick=Delete("/AdminPanel/society/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                    <i class="fas fa-trash-alt"></i>
                    </a>
                    </div>


`
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