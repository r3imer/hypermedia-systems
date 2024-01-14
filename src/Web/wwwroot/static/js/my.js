function sweetConfirm(elt, config) {
    Swal.fire(config)
        .then((result) => {
            if (result.isConfirmed) {
                console.log("sweet confirm fire confirmed");
                elt.dispatchEvent(new Event('debug'));
            }
        });
}
