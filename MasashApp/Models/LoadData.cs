

using Newtonsoft.Json;

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
            if (review_res != "")
            {
                if (review_res[0] != '{')
                {
                    var parsing_review = StaticData.API.ParsingListData(review_res);
                    for (int j = 0; j < parsing_review.Count; j++)
                    {
                        var item_review = parsing_review[j];
                        Model_review review = new Model_review()
                        {
                            _id = item_review["_id"],
                            Name = item_review["name"],
                            Description = item_review["decription"],
                            Star = Convert.ToInt32(item_review["star"]),
                            Date = StaticData.API.GetDateFromString(item_review["date"]),
                        };
                        master.Reviews.Add(review);
                    }
                }
                else
                {
                    var parsing_review = StaticData.API.ParsingData(review_res);

                    var Name = parsing_review["name"];
                    var Description = parsing_review["decription"];
                    var Star = Convert.ToInt32(parsing_review["star"]);
                    var d = StaticData.API.GetDateFromString(parsing_review["date"]);

                    Model_review review = new Model_review()
                    {
                        _id = parsing_review["_id"],
                        Name = parsing_review["name"],
                        Description = parsing_review["decription"],
                        Star = Convert.ToInt32(parsing_review["star"]),
                        Date = StaticData.API.GetDateFromString(parsing_review["date"]),
                    };
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
                if (appointment_res[0] != '{')
                {
                    var parsing_appointment = StaticData.API.ParsingListData(appointment_res);
                    for (int j = 0; j < parsing_appointment.Count; j++)
                    {
                        var item_appointment = parsing_appointment[j];
                        Model_appointment_date appointment = new Model_appointment_date()
                        {
                            _id = item_appointment["_id"],
                            Date = StaticData.API.GetDateFromString(item_appointment["date"]),
                            Master = master,
                            User = new Model_user()
                            {
                                Name = "Таня"
                            }
                        };
                        master.Appointment_dates.Add(appointment);
                    }
                }
                else
                {
                    var parsing_appointment = StaticData.API.ParsingData(appointment_res);
                    Model_appointment_date appointment = new Model_appointment_date()
                    {
                        _id = parsing_appointment["_id"],
                        Date = StaticData.API.GetDateFromString(parsing_appointment["date"]),
                        Master = master,
                        User = new Model_user()
                        {
                            Name = "Таня"
                        }
                    };
                    master.Appointment_dates.Add(appointment);
                }
            }
        }

        public async Task LoadDataSchedule(Model_Master master)
        {
            var schedule_res = await StaticData.API.POST("/get_schedule", new Dictionary<string, string>()
            {
                { "masterId", master._id }
            });

            if (schedule_res != "")
            {
                if (schedule_res[0] != '{')
                {
                    var parsing_schedule = StaticData.API.ParsingListData(schedule_res);
                    for (int j = 0; j < parsing_schedule.Count; j++)
                    {
                        var item_schedule = parsing_schedule[j];
                        Model_schedule schedule = new Model_schedule();
                        schedule._id = item_schedule["_id"];
                        schedule.Date = StaticData.API.GetDateFromString(item_schedule["date"]);
                        schedule.SetDiaposonTimes("10:00", "20:00", 30);
                        master.Schedules.Add(schedule);
                    }
                }
                else
                {
                    var parsing_schedule = StaticData.API.ParsingData(schedule_res);
                    Model_schedule schedule = new Model_schedule();
                    schedule._id = parsing_schedule["_id"];
                    schedule.Date = StaticData.API.GetDateFromString(parsing_schedule["date"]);
                    schedule.SetDiaposonTimes("10:00", "20:00", 30);
                    master.Schedules.Add(schedule);
                }

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

                var values = JsonConvert.DeserializeObject<List<Dictionary<string, Object>>>(CategoryServices_res);

                for (int i = 0; i < values.Count; i++)
                {
                    Dictionary<string, Object> item = values[i];
                    if (item != null)
                    {
                        Model_service_catogory catogory = new Model_service_catogory();
                        catogory._id = item["_id"].ToString();
                        catogory.Name = item["name"].ToString();
                        catogory.PathImg = item["pathImg"].ToString();

                        catogory.Items = new System.Collections.ObjectModel.ObservableCollection<Model_service_item>();

                        var services = JsonConvert.DeserializeObject<List<Dictionary<string, Object>>>(item["services"].ToString());

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
                        }
                    }

                    //Получение всех отзывов на конкретного мастера
                    await LoadDataReview(master);
                    //Кто записан на прием
                    await LoadDataAppointment(master);
                    //Расписание мастера
                    await LoadDataSchedule(master);

                    await LoadDataCategoryServices(master);

                    StaticData.Masters.Add(master);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public List<Dictionary<string, Object>> KeyValues(string json)
        {
            List<Dictionary<string, Object>> result = new List<Dictionary<string, Object>>();

            if(json != null && json != "")
            {

            }

            return result;
        }
    }
}
