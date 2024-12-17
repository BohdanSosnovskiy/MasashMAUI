const net = require("net")
var sokets = []
const tcpServer = net.createServer()

tcpServer.on('connection', newClient)

function newClient(socket)
{
    sokets.push({
        socket,
        remoteAddress: socket.remoteAddress,
        remotePort: socket.remotePort,
        id_client:  '',
        isSendFile: false,
        filename: '',
        filedata_byte: new Buffer(0),
        index: sokets.length - 1
    })

    socket.on("data", function (data) {

        console.log(`client sent ${data}`)
        current_socket = getSocket(socket.remoteAddress, socket.remotePort)
        var data_string = data.toString()

        if(data_string.indexOf('InitUser') > -1)
        {
            sokets[current_socket.index].id_client = data_string.split(':')[0]
        }
        else if(data_string.indexOf('StartSendFile') > -1)
        {
            var filename = data_string.split('|')[1]
            sokets[current_socket.index].filename = filename
            sokets[current_socket.index].filedata_byte = new Buffer(0)
            sokets[current_socket.index].isSendFile = true
        }
        else if(data_string.indexOf('EndSendFile') > -1)
        {
            sokets[current_socket.index].isSendFile = false
            fs.writeFileSync('files/' + sokets[current_socket.index].filename,Buffer.from(sokets[current_socket.index].filedata_byte))
        }
        else if(current_socket.isSendFile)
        {
            sokets[current_socket.index].filedata_byte = Buffer.concat([sokets[current_socket.index].filedata_byte, new Buffer(data, 'binary')])
        }
        else
        {
            SendAllData("data")
        }
    })

    socket.on('close', function(data) 
    {
        let index = sokets.findIndex(function(o) 
        {
            return o.remoteAddress === socket.remoteAddress && o.remotePort === socket.remotePort
        })
        if (index !== -1)
        {
            sokets.splice(index, 1)
        }
        console.log("Clients: " + sokets.length)
        console.log('CLOSED: ' + socket.remoteAddress + ' ' + socket.remotePort)
    })
}

function getSocket(address,port)
{
    for(let i = 0; i < sokets.length; i++)
    {
        let socket = sokets[i]
        if(socket.remoteAddress === address && socket.remotePort === port)
        {
            sokets[i].index = i
            return sokets[i]
        }
    }
}

function SendAllData(message)
{
    for(let i = 0; i < sokets.length; i++)
    {
        sokets[i].socket.write(message)
        console.log(`message sent to client ${i+1}`)
    }
}

tcpServer.listen(5682, '0.0.0.0', () =>{
    console.log('server started on port 5682')
})

const express = require('express')
const app = express()
const bodyParser = require('body-parser')
const fs = require('fs')
const compression = require('compression')
//const multer = require("multer")
const secret = 'Akjhf42P5_fjk11324'
const http = require('http')

const DataBase = require('./database')
const objectId = require("mongodb").ObjectId
const db_name = "MassageAPP"

app.use(bodyParser.json({limit: '50mb'}))
app.use(bodyParser.urlencoded({ extended: true, limit: '50mb'}))
app.use(express.json())
app.use(compression())
app.use(express.static('files'))
//app.use(multer({ dest: "assets/files" }).single("filedata"))
const httpServer = http.createServer(app)
const cors = require('cors')
const { ObjectId } = require("mongodb")

app.use(cors())

app.post('/api/file-upload', (req, res) => {
    try {
        console.log(req)

        res.status(200).json({ success: "file upload successful" })
    } catch (error) {
        res.status(500).json({ error: error })
    }
})


async function baseInitPage(req,res)
{
    let data = {
        isAdmin:false,
        isMaster:false,
        isAuth: false,
        user:{
            login:''
        }
    }

    let id = req.body.id

    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('Users')

    let user = await db.getFind({_id: new objectId(cookie.c_main), token:cookie.token})
    if(user)
    {
        data.isPartner = user.isPartner
        data.isAuth = true
        data.user = user
    }

    return data
}

app.post('/auth', async function(req,res){
    let body = req.body
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('Users')
    let user = await db.getFind({id: body.id})
    if(user != null)
    {
        res.send(user)
    }
    else
    {
        await db.connectDB()
        db.changeCollection('Users')
        user = {
            id: body.id,
            name: '',
            email: '',
            pathImg: '',
            date_birthday: '',
            date_reg: body.date_reg,
            phone: body.phone,
            isAdmin: false,
            isMaster: false
        }
        await db.insert(user)
        res.send("signin")
    }
})

app.post('/getInfoUser', async function(req,res){
    let body = req.body
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('Users')
    let user = await db.getFind({id: body.id})
    if(user != null)
    {
        res.send(user)
    }
    else
    {
        res.send("error")
    }
})

app.post('/getUserInfoByPhone', async function(req,res){
    let body = req.body
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('Users')
    let user = await db.getFind({phone: body.phone})
    if(user != null)
    {
        res.send(user)
    }
    else
    {
        res.send("error")
    }
})

app.post('/set_user_master', async function(req,res){
    let body = req.body
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('Users')
    let user = await db.update({phone: body.phone}, {pathImg: body.pathImg, isMaster: true})
    if(user != null)
    {
        res.send(user)
    }
    else
    {
        res.send("error")
    }
})

app.post('/add_category_service', async function(req,res){
    let body = req.body
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('CategoryServices')

    let categoryService = await db.getFind({name: body.name})

    if(categoryService != null)
    {
        res.send("error")
    }
    let category = {
        name: body.name,
        pathImg: body.pathImg,
        services: [],
        masterId: body.idMaster
    }
    let obj_category = await db.insert(category)
    let id = obj_category.insertedId
    res.send(id)
})

app.post('/edit_category_service', async function(req,res){
    let body = req.body
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('CategoryServices')

    let categoryService = await db.getFind({_id: new ObjectId(body.categoryId)})

    if(body.isChangeImg == 'true')
    {
        //Удаление изображения категории
        let nameFileCategory = categoryService.pathImg.substring(categoryService.pathImg.lastIndexOf('/')+1)
        if(fs.existsSync("files/"+nameFileCategory))
        {
            fs.unlinkSync("files/"+nameFileCategory)
        }
    }

    //Изменение категории
    let data = {
        name: body.name,
        pathImg: body.pathImg,
        services: [],
        masterId: body.masterId
    }
    console.log(data)
    await db.update({_id: new ObjectId(body.categoryId)}, data)

    res.send()
})

app.post('/remove_category_service', async function(req,res){
    let body = req.body
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('CategoryServices')

    let categoryService = await db.getFind({_id: new ObjectId(body.categoryId)})
    if(categoryService != null && categoryService != undefined)
    {
        //Удаление изображения категории
        let nameFileCategory = categoryService.pathImg.substring(categoryService.pathImg.lastIndexOf('/')+1)
        fs.unlinkSync("files/"+nameFileCategory)
        //Удаление категории из базы
        let id = categoryService._id
        await db.removeAt({_id: new ObjectId(id)})
        //Удаление всех услуг категории
        db.changeCollection('Services')
        let services = await db.getFind({categoryId: body.categoryId})

        if(!Array.isArray(services))
        {
            let mass = []
            mass.push(services)
            services = mass
        }

        for(let i = 0; i < services.length; i++)
        {
            let nameFile = services[i].pathImg.substring(services[i].pathImg.lastIndexOf('/')+1)
            fs.unlinkSync("files/"+nameFile)
            await db.removeAt({_id: new ObjectId(services[i]._id)})
        }
    }
    res.send()
})

app.post('/add_service', async function(req,res){
    let body = req.body
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('Services')

    let categoryService = await db.getFind({name: body.name})

    if(categoryService != null)
    {
        res.send("error")
    }
    let category = {
        name: body.name,
        price: body.price,
        time: body.time,
        pathImg: body.pathImg,
        categoryId: body.idCategory,
        masterId: body.idMaster
    }
    let obj_category = await db.insert(category)
    let id = obj_category.insertedId
    res.send(id)
})

app.post('/add_master', async function(req,res){
    let body = req.body
    
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('Masters')
    await db.insert(body)
    res.send("ok")
})

app.post('/get_masters', async function(req,res){
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('Masters')
    res.send(await db.getAll())
})

app.post('/get_CategoryServices', async function(req,res){
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('CategoryServices')

    let category = await db.getFind({masterId: req.body.masterId})

    db.changeCollection('Services')

    if(category != null && category != undefined)
    {
        if(Array.isArray(category))
        {
            category.services = []
            for(let i = 0; i < category.length; i++)
            {
                let ctgr = category[i]
                let services = await db.getFind({categoryId: ctgr._id.toString()})
                if(services!= null && services!= undefined)
                {
                    if(Array.isArray(services))
                    {
                        category[i].services = services
                    }
                    else
                    {
                        category[i].services.push(services)
                    }
                }
            }
        }
        else
        {
            category.services = []
            let services = await db.getFind({categoryId: category._id.toString()})
            if(services!= null && services!= undefined)
            {
                if(Array.isArray(services))
                {
                    category.services = services
                }
                else
                {
                    category.services.push(services)
                }
            }
            let mass = []
            mass.push(category)
            category = mass
        }
    }
    res.send(category)
})


app.post('/get_reviews', async function(req,res){
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('Reviews')
    res.send(await db.getFind({masterId: req.body.masterId}))
})

app.post('/get_appointment', async function(req,res){
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('Appointment')
    res.send(await db.getFind({masterId: req.body.masterId}))
})

app.post('/get_schedule', async function(req,res){
    let db = new DataBase(undefined,db_name)
    await db.connectDB()
    db.changeCollection('Schedule')
    res.send(await db.getFind({masterId: req.body.masterId}))
})

function replaceAll(from,to,text)
{
    while(text.indexOf(from) > -1)
    {
        text = text.replace(from,to)
    }
    return text
}

function generatePath()
{
    let alf = '1qaz2wsx3edc4rfv5tgb6yhn7ujm8ik9ol0p'
    let result = ''
    for(let i = 0; i < 10; i++)
    {
        result += alf[getRandom(0,alf.length-1)]
    }
    return result
}

function getRandom(min, max) {
    return parseInt(Math.random() * (max - min) + min);
}
function toStringDate(date)
{
    let dates = new Date(date)
    let result = ""

    if(dates.getHours() < 10)
    {
        result += '0' + dates.getHours() + ":"
    }
    else
    {
        result += dates.getHours() + ":"
    }

    if(dates.getMinutes() < 10)
    {
        result += '0'+dates.getMinutes() + ' '
    }
    else
    {
        result += dates.getMinutes() + ' '
    }
    

    if(dates.getDate() < 10)
    {
        result += "0"+dates.getDate() + "."
    }
    else
    {
        result += dates.getDate()+ "."
    }
    if(dates.getMonth() < 10)
    {
        result += "0"+dates.getMonth()+ "."
    }
    else
    {
        result += dates.getMonth()+ "."
    }
    result += dates.getFullYear()
    return result
}

httpServer.listen(4362, () => { 
    console.log('HTTP Server running on port http://127.0.0.1:4362')
})