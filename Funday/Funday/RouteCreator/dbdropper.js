const config = {
    user: 'Fun Login',
    password: 'ILoveFun1',
    port: 5432,
    host: '35.236.206.69'
}

const pgtools = require('pgtools');
pgtools.dropdb(config, 'fun_dev', function (err, res) {
    if (err) {
        //    console.error(err);
        //   process.exit(-1);
    }
    //   console.log(res);

    pgtools.createdb(config, 'fun_dev', function (err, res) {
        if (err) {
            console.error(err);
            process.exit(-1);
        }
        console.log(res);
    });
});
