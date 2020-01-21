const fs = require('fs');
const config = JSON.parse(fs.readFileSync('./config.json', 'utf-8'));
const TwoCaptchaKey = config.TwoCaptchaKey;

const Nightmare = require('nightmare');
// @ts-ignore
// const Addon = require('nightmare-webrequest-addon');
const uuid = require('uuid');
const EmailSelector = '#email-login';
const PasswordSelector = '#password-login';
const args = require('args');
const LoginButtonSelector = '#btn-login';
const path = require('path');
const UserProfileDirectory = require('temp-dir');
const fetch = require('node-fetch');
const rimraf = require('rimraf');

export class NightMareStocks {
    Options: IArgs;
    CaptchaProblems: number = 0;
    NonCaptchaProblems: number = 0;
    stonks: string;
    Browser: any;

    constructor(Options: IArgs) {
        this.Options = Options;
        this.CaptchaProblems = 0;
        this.NonCaptchaProblems = 0;

    }

    async LoginStockx(): Promise<any> {

        this.stonks = uuid.v4();
        const SafeVersionOfLogin = this.stonks.replace(/[^a-zA-z]/, '');

        this.Browser = new Nightmare({
            switches: {
               // 'proxy-server': this.Options.proxyhost !== undefined ? `${this.Options.proxyhost}:${this.Options.port}` : undefined,
             //   'ignore-certificate-errors': true,
            },
            paths: {
                userData: UserProfileDirectory + path.sep + SafeVersionOfLogin,
            },
            //   openDevTools: {detach: true},
            webPreferences: {
                images: false ,
                partition: this.stonks,
            },
            weak: false,
            maxAuthRetries: 3,
            show: true  ,
          //  ignoreSslErrors: true,
           // sslProtocol: 'tlsv1',
            waitTimeout: 30000,
            gotoTimeout: 30000,
        });
        if (this.Options.proxyusername !== undefined) {
            //await this.Browser.authentication(this.Options.proxyusername, this.Options.proxypassword);
        }
        await this.Browser.useragent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");


        let Data = null;
        let AccessToken = null;

        try {


            await this.Browser.goto('https://accounts.stockx.com/login');

 
            let IsCaptcha = await this.DetectIfCaptcha();
            if (IsCaptcha && IsCaptcha.SiteKey) {
                await this.DoCaptcha(IsCaptcha);
                await this.CloseOut();
                if (this.CaptchaProblems++ > 10) {
                    return;
                }
                return await this.LoginStockx();
            }

 
            console.log(this.Options);
        //    
               await this.Browser.click('#nav-login');
        //    await this.Browser.click('#site-header > div > nav > div > div.navbar-inner > ul > li:nth-child(7) > a');
            await this.Browser.wait(EmailSelector);
            await this.Browser.click(EmailSelector);
            await this.Browser.type(EmailSelector, this.Options.username);
            await this.Browser.click(PasswordSelector);
            await this.Browser.type(PasswordSelector, this.Options.password);
            await this.Browser.click(LoginButtonSelector);


            for (let i = 0; i < 2; i++) {

                await this.CheckForInvalidPw();

                try {
                    IsCaptcha = await this.DetectIfCaptcha();
                    if (IsCaptcha && IsCaptcha.SiteKey) {
                        await this.DoCaptcha(IsCaptcha);
                        await this.CloseOut();
                        if (this.CaptchaProblems++ > 10) {
                            return;
                        }
                    }
                    await this.Browser.wait('[href="/account"]');

                    const Cookies = await this.Browser.cookies.get();

                    const TokenCookie = Cookies.find((A: any) => A.name === 'token');
                    if (TokenCookie) {
                        AccessToken = TokenCookie.value;
                    }
                    // @ts-ignore
                    const CustomerInfoProps = await this.Browser.evaluate(() => window.preLoadedBaseProps || window.preLoadedHomeProps);
                    if (!CustomerInfoProps) {

                        console.log('Couldn\'t get Home or base props.');
                        await this.CloseOut();
                        return {
                            error: 'Couldn\'t get Home or base props.'
                        };
                    }
                    const CustomerInfo = CustomerInfoProps.customer;
                    // @ts-ignore
                    const Globals = await this.Browser.evaluate(() => window.globalConstants);
                    if (CustomerInfo && Globals) {
                        Data = {AccessToken, Cookies, CustomerInfo, Globals};
                        break;
                    }
                    await this.CloseOut();
                    return Data;
           
                } catch (e) {
                    console.debug(`${this.Options.username} Could not Get account, retrying`, e);
                    await this.CloseOut();
                    return {
                        error: e.message
                    };
                }
            }


            if (Data === null) {
                console.info(`${this.Options.username} but could not get tokens`);
            }
            await this.CloseOut();
            this.CaptchaProblems = 0;
            return Data;


        } catch (e) {


            if (e.message === 'Invalid Username or Password') {
                await this.Browser.screenshot(`./errors/invaliduserpass_${this.stonks}.png`);
                await this.Browser.html(path.resolve(`./errors/invaliduserpass_${this.stonks}.html`));
                await this.CloseOut();
                return {
                    error: e.message
                };
            }
            try {
            //    await this.Browser.wait('[class*=\'badGateway__Title\']');
              //  console.log('Stockx pooping itself detected');
//                await this.CloseOut();
  //              return {
    //                error: 'Stockx too busy'
      //          };

            } catch (e) {

            }

            await this.DoUnKnownError(e);

            await this.CloseOut();

            return {
                error: e.message
            };
        }


    }

    async CheckForInvalidPw() {

        
            
            const invalidpw = await this.Browser.evaluate(() => {
                const errorcontainer = document.querySelector('#error-message');
                if (!errorcontainer) {
                    return null;
                }
                return errorcontainer.textContent && errorcontainer.textContent.match(/Incorrect email or password/i);
            });

            if (invalidpw) {
                console.error(this.Options.username + ' has invalid password. Please fix.');

                throw new Error('Invalid Username or Password');

            }
        
    }

    async DetectIfCaptcha() {
        try {
     
            const captcha = await this.Browser.evaluate(() => {
                const captcha = document.querySelector('[data-sitekey]');
                if (captcha) {
                    return {
                        SiteKey: captcha.getAttribute('data-sitekey'),
                        CallBack: captcha.getAttribute('data-callback'),
                        Url: document.location.href,
                        TextBoxExists: !!document.querySelector('#g-recaptcha-response'),
                    };

                }
                
                return null;
            });
            return captcha;
        } catch (e) {
            {
                
            }
        }


    }

    async DoCaptcha(captcha: any) {
        this.CaptchaProblems++;
        if (this.CaptchaProblems > 20) {
            console.debug(this.Options.username + ' There might be a change in 2captcha or the captcha page changed');
            return;

        }


        const SiteKey = captcha.SiteKey;
        const requestUrl = `http://2captcha.com/in.php?key=${encodeURIComponent(TwoCaptchaKey)}&method=userrecaptcha&googlekey=${encodeURIComponent(SiteKey)}&pageurl=${encodeURIComponent('https://stockx.com')}`;
        try {
            console.debug('fetching 2captcha', requestUrl);
            const response = await fetch(requestUrl);
            const txt = await response.text();
            console.debug('fetching 2captcha got ', txt);
            if (txt.length < 3) {
                console.debug(this.Options.username + ': something wrong with 2captcha');
                return;
            }
            if (txt.substr(0, 3) !== 'OK|') {
                console.error(this.Options.username + ' Could not get response from 2captcha, could be empty account');
                this.CaptchaProblems += 5;
                return;
            }
            const captchaId = txt.substr(3);
            console.debug(this.Options.username + ': fetching 2captcha got id ', captchaId);
            for (let i = 0; i < 124; i++) {
                const Answer = await this.GetCaptchaanswer(captchaId);

                if (Answer) {
                    await this.Browser.evaluate((Answer: any, captcha: any) => {

                        // @ts-ignore
                        document.querySelector('#g-recaptcha-response').value = Answer;
                        // @ts-ignore
                        window[captcha.CallBack](Answer);
                    }, Answer, captcha);

                    await timeout(20000);
                    return;
                }
            }


        } catch (e) {
            console.error(this.Options.username + ': fetch error to 2captcha', e);
            return null;
        }

    }

    async GetCaptchaanswer(captchaId: any) {
        await timeout(5000);
        const ansUrl = `http://2captcha.com/res.php?key=${encodeURIComponent(TwoCaptchaKey)}&action=get&id=${encodeURIComponent(captchaId)}`;
        const answerresponse = await fetch(ansUrl);
        const answertext = await answerresponse.text();
        console.debug(this.Options.username + ': fetching 2captcha got answer ', answertext);
        if (answertext.match(/ERROR_CAPTCHA_UNSOLVABLE/)) {
            return null;
        }
        if (answertext.substr(0, 3) === 'OK|') {
            return answertext.substr(3);
        }
        return null;
    } 

    async DoUnKnownError(e: any) {
        this.NonCaptchaProblems++;
        if (this.NonCaptchaProblems > 10) {
            console.error(this.Options.username + ': Non captcha error found, contact developer if this persists. Loop detected. Shutting down.');
            await this.Browser.screenshot(`./errors/${this.stonks}.png`);
            await this.Browser.html(path.resolve(`./errors/${this.stonks}.html`));
            await this.CloseOut();
            process.exit(10);

        }
        console.error('Non captcha error found, contact developer if this persists. Sometimes it\'s just network delays. Might be a changed form.', e);
        await this.Browser.screenshot(`./errors/${this.stonks}.png`);
        await this.Browser.html(path.resolve(`./errors/${this.stonks}.html`));
    }

    async CloseOut() {
        const SafeVersionOfLogin = this.stonks.replace(/[^a-zA-z]/, '');
        await this.Browser.screenshot(this.Options.jobId + ".png");
        await this.Browser.end();
        rimraf.sync(UserProfileDirectory + path.sep + SafeVersionOfLogin);
    }


}


export const timeout = (ms: number) => new Promise((res) => setTimeout(res, ms));
export class StockXAccount {

    Id: number;
    Email: string;
    Password: string;
    ProxyUsername: string;
    ProxyPassword: string;
    ProxyHost: string;
    ProxyPort: number;
    ProxyActive: boolean;
    Active: boolean;
    CustomerID: string;
    Currency: string;
    Country: string;
    UserAgent: string;
    Token: string;

}
export interface IArgs {
    baseJson: string;
    username: string;
    password: string;
    typeproxy: string;
    proxyhost: string;
    proxyusername: string;
    userAgent: string;
    proxypassword: string;
    port: number;
    jobId: string;
}

args.options([
    { name: 'BaseJson', description: 'The base64 for input' },
    { name: 'JobID', description: 'The Job Id for output' },
]);

const Options :IArgs = args.parse(process.argv);
console.log(Options);
let buff = new Buffer(Options.baseJson, 'base64');
let text = buff.toString('ascii');
console.log(text);
try {
    let Json: StockXAccount = JSON.parse(text);
    Options.username = Json.Email;
    Options.password = Json.Password;
    Options.proxyhost = Json.ProxyHost;
    Options.proxypassword = Json.ProxyPassword;
    Options.proxyusername = Json.ProxyUsername;
    Options.port = +Json.ProxyPort;
    Options.userAgent = Json.UserAgent;
    


} catch (e) {
    fs.writeFileSync(`${Options.jobId}`, JSON.stringify({ error: "Bad input json " + e.message  }), 'utf-8');
    process.exit();
}


    console.log(  `${Options.jobId}`);

    const a = new NightMareStocks(Options);
    a.LoginStockx().then(Data => {
        console.log(Data);
        fs.writeFileSync(  `${Options.jobId}`, JSON.stringify(Data), 'utf-8');
    });



