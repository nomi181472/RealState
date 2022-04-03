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
                "data":"userName","Width":"10%"
            },
            {
                "data": "email", "Width": "10%"
            },
            
            {
                "data": "phoneNumber", "Width": "10%"
            },
            {
                "data": "isVerified",
                "render": function (data) {
                    if (data) {
                        return `YES`
                    }
                    else {
                        return `NO`
                    }
                },
                "width": "10%"
            },
           
            {
                "data": {
                    id: "id", lockoutend: "lockoutEnd", isVerified:"isVerified"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                    <div class="text-center">
                    <a   class="btn btn-danger text-white" style="cursor:pointer">
                        <i class="fa-solid fa-check" ></i>
                    </a>
                    <a  onclick=ClickToUnlock("${data.id}") class="btn btn-danger text-white" style="cursor:pointer">
                    <i class="fas fa-lock-open"></i> Unlock
                    </a>
                    </div>
                        `
                    }
                    else {
                        var str =""
                        if (data.isVerified) {
                            str=`
                            <a onclick=ClickToVerify("${data.id}") class="btn btn-success text-white" style="cursor:pointer">
                                                <i class="fa-solid fa-xmark"></i>
                                                </a>
                                `
                        }
                        else {
                            str = `
                                 <a onclick=ClickToVerify("${data.id}") class="btn btn-danger text-white" style="cursor:pointer">
                                                <i class="fa-solid fa-check"></i>
                                                </a>
                                `
                        }
                        

                        return `
                    <div class="text-center">
                    ${str}
                    <a  onclick=ClickToUnlock("${data.id}") class="btn btn-success text-white" style="cursor:pointer">
                    <i class="fas fa-lock"></i> Lock
                    </a>
                    </div>`
                    }
                    
                },
                "width":"40%"
            }
        ]
    })
}


/*function Delete(url) {
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
}*/
function ClickToUnlock(id) {


            $.ajax(
                {
                    type: "POST",
                    url: '/AdminPanel/VerifiedUser/LockOrUnlock',
                    data: JSON.stringify(id),
                    contentType:"application/json",
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
function ClickToVerify(id) {
 

            $.ajax(
                {
                    type: "POST",
                    url: '/AdminPanel/VerifiedUser/ModifyRole',
                    data: JSON.stringify(id),
                    contentType:"application/json",
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
        
