

using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace MasashApp.Models
{
    public class LoadData
    {
        public LoadData() 
        {

        }

        /// <summary>
        /// Получение всех отзывов на конкретного мастера
        /// </summary>
        /// <param name="master">Обьект мастера в который нужно загрузить его отзывы</param>
        /// <returns></returns>
        public async Task LoadDataReview(Model_Master master)
        {
            //Получение всех отзывов на конкретного мастера
            var review_res = await StaticData.API.POST("/get_reviews", new Dictionary<string, string>()
            {
                { "masterId", master._id }
            });
            if(review_res != "")
            {
                var parsing_review = StaticData.API.ParsingListData(review_res);
                for (int j = 0; j < parsing_review.Count; j++)
                {
                    var item_review = parsing_review[j];
                    Model_review review = new Model_review();
                    review._id = item_review["_id"];
                    review.Name = item_review["name"];
                    review.AppointmentId = item_review["appointmentID"];
                    review.Description = item_review["description"];
                    review.Star = Convert.ToInt32(item_review["star"]);
                    review.Date = StaticData.API.GetDateFromString(item_review["date"]);
                    master.Reviews.Add(review);
                }
            }
            
        }
        /// <summary>
        /// Загрузка списка пользователей кто записан на прием
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        public async Task LoadDataAppointment(Model_Master master)
        {
            var appointment_res = await StaticData.API.POST("/get_appointment", new Dictionary<string, string>()
            {
                { "masterId", master._id }
            });

            if (appointment_res != "")
            {

                var parsing_appointment = StaticData.API.ParsingListData(appointment_res);
                StaticData.User.Appointment_dates = new ObservableCollection<Model_appointment_date>();
                master.Appointment_dates = new ObservableCollection<Model_appointment_date>();
                for (int j = 0; j < parsing_appointment.Count; j++)
                {
                    var item_appointment = parsing_appointment[j];

                    ObservableCollection<Model_service_item> service_Items = new ObservableCollection<Model_service_item>();

                    string[] id_mass = item_appointment["services"].Split(',');
                    for (int i = 0; i < id_mass.Length; i++)
                    {
                        var id = id_mass[i];
                        bool isFind = false;
                        for (int k = 0; k < master.Catergory_Sevice.Count; k++)
                        {
                            if (!isFind)
                            {
                                var category = master.Catergory_Sevice[k];
                                for (int l = 0; l < category.Items.Count; l++)
                                {
                                    var service = category.Items[l];
                                    if (service._id == id)
                                    {
                                        service_Items.Add(service);
                                        isFind = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    var res_user = await StaticData.API.POST("/getInfoUser", new Dictionary<string, string>()
                    {
                        { "id", item_appointment["userId"] }
                    });

                    if (res_user != "")
                    {
                        Dictionary<string, string> parsing = StaticData.API.ParsingData(res_user);

                        Model_appointment_date appointment = new Model_appointment_date()
                        {
                            _id = item_appointment["_id"],
                            Date = StaticData.API.GetDateFromString(item_appointment["date"]),
                            Master = master,
                            User = new Model_user()
                            {
                                PathImg = parsing["pathImg"],
                                Name = parsing["name"],
                                Email = parsing["email"],
                                Date_birthday = StaticData.API.GetDateFromString(parsing["date_birthday"]),
                                Date_reg = StaticData.API.GetDateFromString(parsing["date_reg"]),
                                IsAdmin = Convert.ToBoolean(parsing["isAdmin"]),
                                IsMaster = Convert.ToBoolean(parsing["isMaster"]),
                                Phone = parsing["phone"]
                            },
                            Catergory_Item_Sevice = service_Items
                        };

                        if(appointment.User.Phone == StaticData.User.Phone)
                        {
                            StaticData.User.Appointment_dates.Add(appointment);
                        }

                        master.Appointment_dates.Add(appointment);
                    }
                }
            }
        }

        public async Task LoadDataSchedule(Model_Master master)
        {
            var schedule_res = await StaticData.API.POST("/get_schedule", new Dictionary<string, string>()
            {
                { "masterId", master._id }
            });


            var parsing_schedule = StaticData.API.ParsingListData(schedule_res);
            for (int j = 0; j < parsing_schedule.Count; j++)
            {
                var item_schedule = parsing_schedule[j];
                Model_schedule schedule = new Model_schedule();
                schedule._id = item_schedule["_id"];
                schedule.Date = StaticData.API.GetDateFromString(item_schedule["date"]);
                schedule.SetDiaposonTimes(item_schedule["fromTime"], item_schedule["toTime"], 30);
                master.Schedules.Add(schedule);
            }
        }

        public async Task LoadDataCategoryServices(Model_Master master)
        {
            var CategoryServices_res = await StaticData.API.POST("/get_CategoryServices", new Dictionary<string, string>()
            {
                { "masterId", master._id }
            });

            if (CategoryServices_res != "")
            {
                master.Catergory_Sevice = new System.Collections.ObjectModel.ObservableCollection<Model_service_catogory>();

                var values = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(CategoryServices_res);

                for (int i = 0; i < values.Count; i++)
                {
                    Dictionary<string, object> item = values[i];
                    if (item != null)
                    {
                        Model_service_catogory catogory = new Model_service_catogory();
                        catogory._id = item["_id"].ToString();
                        catogory.Name = item["name"].ToString();
                        catogory.PathImg = item["pathImg"].ToString();

                        catogory.Items = new System.Collections.ObjectModel.ObservableCollection<Model_service_item>();

                        var services = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(item["services"].ToString());

                        for (int j = 0; j < services.Count; j++)
                        {
                            var item_service = services[j];
                            Model_service_item service = new Model_service_item();
                            service._id = item_service["_id"].ToString();
                            service.Name = item_service["name"].ToString();
                            service.Time = Convert.ToInt32(item_service["time"]);
                            service.PathImg = item_service["pathImg"].ToString();
                            service.Price = Convert.ToInt32(item_service["price"]);
                            service.LinkModelCategory = catogory;

                            catogory.Items.Add(service);
                        }

                        master.Catergory_Sevice.Add(catogory);
                    }
                }
            }
        }

        /// <summary>
        /// Загрузка всех данных мастера
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataMasters()
        {
            try
            {
                StaticData.Masters = new ObservableCollection<Model_Master>();
                var result = await StaticData.API.POST("/get_masters", new Dictionary<string, string>());
                List<Dictionary<string, string>> parsing = StaticData.API.ParsingListData(result);
                Console.WriteLine();
                for (int i = 0; i < parsing.Count; i++)
                {
                    var item = parsing[i];
                    Model_Master master = new Model_Master()
                    {
                        _id = item["_id"],
                        Name = item["name"],
                        PathImg = item["pathImg"],
                        Phone = item["phone"]
                    };

                    if(StaticData.User != null)
                    {
                        if(StaticData.User.Phone == master.Phone)
                        {
                            StaticData.User.Master = master;
                            StaticData.User.IsMaster = true;
                        }
                    }

                    //Получение всех отзывов на конкретного мастера
                    await LoadDataReview(master);
                    
                    //Расписание мастера
                    await LoadDataSchedule(master);

                    //Категории и услуги
                    await LoadDataCategoryServices(master);

                    //Кто записан на прием
                    await LoadDataAppointment(master);

                    StaticData.Masters.Add(master);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public List<Dictionary<string, object>> KeyValues(string json)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            if(json != null && json != "")
            {

            }

            return result;
        }
    }
}
