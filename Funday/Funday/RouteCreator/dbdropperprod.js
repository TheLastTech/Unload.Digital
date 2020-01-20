const config = {
    user: 'postgres',
    password: 'tH8dfdsf4ga.PET7Xu',
    port: 5432,
    host: 'localhost'
};

const pgtools = require('pgtools');
pgtools.dropdb(config, 'bloodorange', function (err, res) {
    if (err) {
        //    console.error(err);
        //   process.exit(-1);
    }
    //   console.log(res);

    pgtools.createdb(config, 'bloodorange', function (err, res) {
        if (err) {
            console.error(err);
            process.exit(-1);
        }
        console.log(res);
    });
});
