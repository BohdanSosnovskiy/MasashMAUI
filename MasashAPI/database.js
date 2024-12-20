const MongoClient = require("mongodb").MongoClient
const objectId = require("mongodb").ObjectID

module.exports = class DataBase{
    
    /**
     * Инициализация бд
     */
    constructor(db,nameDB)
    {
        if(db == undefined)
        {
            this.db_name = nameDB
            this.mongoClient = new MongoClient("mongodb://127.0.0.1:27017/", { })
        }
        else
        {
            this.db_name = db.db_name
            this.mongoClient = db.mongoClient
            this.client = db.client
            this.db = db.db
            this.collection = db.collection
        }
        this.isClose = true
    }

    async connectDB()
    {
        this.client = await this.mongoClient.connect()
        this.db = this.client.db(this.db_name)
        this.isClose = false
    }

    /**
     * Удаление обьекиа из колекции
     * @param {*} filter 
     */
    async remove(filter)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            var r = await this.collection.deleteMany(filter)
        }
        catch(ex)
        {
            console.error(ex)
        }
        finally
        {
            await this.client.close()
            this.isClose = true
        }

        return r
    }
    /**
     * Получение всех колекций
     * @returns 
     */
    async getAllCollectionsNames()
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            let col = []
            let show = await this.db.collections()
            
            for(let i = 0; i < show.length; i++)
            {
                try
                {
                    col.push(show[i].collectionName)
                }
                catch(ex)
                {
                    console.error(ex)
                }
                
            }
            return col
        }
        catch(ex)
        {
            console.error(ex)
        }
        finally
        {
            await this.client.close()
            this.isClose = true
        }
        
    }

    async listDBS()
    {
        let admin = this.client.db(this.db_name).admin()
        this.dbs = await admin.listDatabases()
        return this.dbs
    }

    /**
     * Удаление обьекиа из колекции
     * @param {*} filter 
     */
    async removeAt(filter)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            var r = await this.collection.deleteOne(filter)
        }
        catch(ex)
        {
            console.error(ex)
        }
        finally
        {
            await this.client.close()
            this.isClose = true
        }
        return r
    }

    /**
     * Изменяет текщую колекцию
     * @param {*} name Имя нужной колекции
     */
    changeCollection(name)
    {
        this.collection = this.db.collection(name)
    }

    /**
     * Загрузка данных в базу (только один обьект) даже если массив
     * @param {*} data Объект загружаемый в коллекцию
     */
    async insert(data)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            let result = await this.collection.insertOne(data)
            return result
        }
        catch(err)
        {
            console.error(err)
            return false
        }
        finally
        {
            this.client.close()
            this.isClose = true
        }
    }
    /**
     * Загрузка данных в базу
     * @param {*} data Массив даных который нужно загрузить
     */
    async insertMany(data)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            let result = await this.collection.insertMany(data)
            return result
        }
        catch(err)
        {
            console.error(err)
            return false
        }
        finally
        {
            this.client.close()
            this.isClose = true
        }
    }

    async regUser(user)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            this.changeCollection('Users')
            let users = await this.getAll()
            let id = 0
            if(users.length > 0)
            {
                id = users[users.length - 1].numId + 1
            }
            if(await this.getFind({email: user.email}) == null)
            {
                user.numId = id
                user.dateReg = new Date()
                user.countBuys = 0 //Количество покупок
                user.isAdmin = false
                
                await this.insert(user)
                return true
            }
            else
            {
                return false
            }
        }
        catch(ex)
        {
            console.error(ex)
        }
        finally
        {
            this.client.close()
            this.isClose = true
        }
        
    }
    /**
     * Получение данных пользователя
     * @param {*} email указанный при регестрации
     */
    async getUser(email)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            this.changeCollection('Users')
            let result = await this.getFind({email})
            if(result != null)
            {
                return result
            }
            else
            {
                return null
            }
        }
        catch(ex)
        {
            console.error(ex)
        }
        finally
        {
            this.client.close()
            this.isClose = true
        }
        
    }

    /**
     * Получение данных пользователя
     * @param {*} id chatId пользователя в телеграм
     */
    async getUserTelegram(id)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        this.changeCollection('Users')
        let result = await this.getFind({chatId:id})
        if(result != null)
        {
            return result
        }
        else
        {
            return null
        }
    }

    /**
     * Сохранение данных пользователя в бд
     * @param {*} chatId id пользователя
     * @param {*} data объект который нужно сохранить
     */
    async saveUserData(chatId,data)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        this.changeCollection("storage")
        let f = await this.getFind({chatId})
        if(f != null)
        {
            if(this.isClose)
            {
                await this.connectDB()
            }
            await this.collection.findOneAndUpdate(
            {chatId: chatId}, // критерий выборки
            { $set: data},     // параметр обновления
            {                           // доп. опции обновления    
                returnOriginal: false
            })
        }
        else
        {
            data.chatId = chatId
            this.insert(data)
        }
    }
    /**
     * Получение данных пользователя
     * @param {*} chatId id пользователя
     */
    async getUserData(chatId)
    {
        this.changeCollection("storage")
        return await this.getFind({chatId})
    }

    /**
     * Обновление документа в коллекции
     * @param {*} filter критерий выборки
     * @param {*} data данные которые нужно обновить
     */
    async update(filter,data)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            return await this.collection.findOneAndUpdate(
                filter, // критерий выборки
                { $set: data},     // параметр обновления
                {                           // доп. опции обновления    
                    returnOriginal: false
                })
        }
        catch(ex)
        {
            console.error(ex)
        }
        finally
        {
            this.client.close()
            this.isClose = true
        }
        
    }

    /**
     * Обновление документа в коллекции
     * @param {*} filter критерий выборки
     * @param {*} data данные которые нужно обновить
     */
    async updateMany(filter,data)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            return await this.collection.updateMany(
            filter, // критерий выборки
            { $set: data},     // параметр обновления
            {                           // доп. опции обновления    
            returnOriginal: false
            })
        }
        catch(ex)
        {
            console.error(ex)
        }
        finally
        {
            this.client.close()
            this.isClose = true
        }
    }

    /**
     * Получение информации о пользователе в виде строки
     * @param {*} chatId 
     */
    async getUserString(chatId)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            this.changeCollection('Users')
            let result = await this.getFind({chatId})
            if(result != null)
            {
                let text = "Ваш балланс: " + result.ballans + "\n"
                text += "Ваш ID: " + result.chatId
                return text
            }
            else
            {
                return ""
            }
        }
        catch(ex)
        {
            console.error(ex)
        }
        finally
        {
            this.client.close()
            this.isClose = true
        }
    }

    /**
     * Сохранение выборов пользователя
     * @param {*} chatId id пользователя
     * @param {*} data данные для сохранения
     */
    async tempChoise(chatId,data)
    {
        this.changeCollection('tempChoise')
        let f = await this.getFind({chatId})
        if(f != null)
        {
            await this.collection.findOneAndUpdate(
            {chatId: chatId}, // критерий выборки
            { $set: data},     // параметр обновления
            {                           // доп. опции обновления    
                returnOriginal: false
            })
        }
        else
        {
            data.chatId = chatId
            this.insert(data)
        }
    }

    /**
     * Получение всех данных колекции
     */
    async getAll()
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            return await this.collection.find().toArray()
        }
        catch(ex)
        {
            console.error(ex)
        }
        finally
        {
            this.client.close()
            this.isClose = true
        }
    }

    /**
     * Возвращает объект(ы) коллекции
     * @param {*} filter Объект фильтрации
     */
    async getFind(filter)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            let result = await this.collection.find(filter).toArray()
            if(result.length == 1)
            {
                return result[0]
            }
            else if(result.length > 1)
            {
                return result
            }
            else
            {
                return null
            }
        }
        catch(ex)
        {
            console.error(ex)
        }
        
    }

    /**
     * Возвращает объект(ы) коллекции по параметрам
     * @param {*} filter Объект фильтрации
     * @param {*} options sort тип тортировки {имя поля:число} -1 по убываю 1 по возрастанию, skip:число пропустить первые N документов, limit:число сколько нужно отобразить документов
     * @returns 
     */
    async getFindOptions(filter,options)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            let result = await this.collection.find(filter,options).toArray()
            
            if(result.length == 1)
            {
                return result[0]
            }
            else if(result.length > 1)
            {
                return result
            }
            else
            {
                return null
            }
        }
        catch(ex)
        {
            console.error(ex)
        }
        finally
        {
            this.client.close()
            this.isClose = true
        }
    }

    /**
     * Возвращает объект(ы) коллекции по параметрам
     * @param {*} filter Объект фильтрации
     * @param {*} options sort тип тортировки {имя поля:число} -1 по убываю 1 по возрастанию, skip:число пропустить первые N документов, limit:число сколько нужно отобразить документов
     * @param {*} projection обьект фильтрации то какие будут возвращатся поля в документе {_id: 0, name: 1} где 0 это исключить а 1 включить во все документы
     * @returns 
     */
    async getFindProject(filter,options,projection)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            let result = await this.collection.find(filter,options).project(projection).toArray()
            
            if(result.length == 1)
            {
                return result[0]
            }
            else if(result.length > 1)
            {
                return result
            }
            else
            {
                return null
            }
        }
        catch(ex)
        {
            console.error(ex)
        }
        finally
        {
            this.client.close()
            this.isClose = true
        }
    }

    async getCount(filter)
    {
        if(this.isClose)
        {
            await this.connectDB()
        }
        try
        {
            let result = await this.collection.count(filter)

            return result
        }
        catch(ex)
        {
            console.error(ex)
        }
        finally
        {
            this.client.close()
            this.isClose = true
        }
    }
}