using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace ThrustCurveBrowser
{
    public static class ApiService
    {
        private static readonly string BaseUrl = "http://www.thrustcurve.org/servlets/";
        private static readonly string SearchEndpoint = BaseUrl + "search/";
        private static readonly string DownloadEndpoint = BaseUrl + "download/";
        private static readonly string MetadataEndpoint = BaseUrl + "metadata/";

        private static readonly HttpClient client = new HttpClient();

        public static async Task<bool> TestConnection()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://www.thrustcurve.org");
                return response.IsSuccessStatusCode;
            }
            catch(Exception err)
            {
                Console.WriteLine("Failed to query ThrustCurve homepage");
                Console.WriteLine(err);
            }
            return false;
        }
    }

    /// <summary>
    /// Struct representing a single Rocket Motor
    /// </summary>
    public struct RocketMotor
    {
        /// <summary>
        /// The integer ID of the motor, in ThrustCurve's database
        /// </summary>
        public int motorId;
        /// <summary>
        /// The motor's manufacturer
        /// </summary>
        public string manufacturer;
        public string manufacturerAbbrev;
        /// <summary>
        /// The standard designation for the motor.
        /// </summary>
        /// <remarks>
        /// These usually take the form `{ImpulseClass}{AvgThrustN}{PropType?}-{Delay}`
        /// </remarks>
        public string designation;
        public string brandName;
        /// <summary>
        /// A simplified form of the designation, especially for CTI motors
        /// </summary>
        public string commonName;
        /// <summary>
        /// A letter indicating the impulse class of the motor.
        /// </summary>
        /// <seealso cref="https://en.wikipedia.org/wiki/Model_rocket_motor_classification#Motor_impulse_by_class"/>
        public string impulseClass;
        /// <summary>
        /// The diameter of the motor, in millimeters.
        /// </summary>
        /// <remarks>
        /// Most engines come in standard sizes, such as 29mm or 38mm, but there are a few oddballs
        /// </remarks>
        public float diameter;
        /// <summary>
        /// The length of the motor, in millimeters.
        /// </summary>
        public float length;
        /// <summary>
        /// The type of engine- whether the motor is reusable, single use, or a hybrid rocket.
        /// </summary>
        /// <remarks>
        /// Generally you can expect this to be one of: "hybrid", "reloadable", "SU"; SU meaning "single use".
        /// Do not expect this to be true always and forever- there are already "tribrids" on the market using 3
        /// propellants.
        /// 
        /// While "hybrid" and the other 2 categories are not mutually exclusive, in practice hybrids stand out
        /// as their own category in hobbyist and regulatory parlance. 
        /// </remarks>
        public string type;
        /// <summary>
        /// The organization that first certified this rocket to fly.
        /// </summary>
        /// <remarks>
        /// Multiple agencies might (and often do) test a particular engine- in fact, they might test it multiple
        /// times as well. This only gives the first organization to do so.
        /// </remarks>
        public string certOrg;
        /// <summary>
        /// The motor's average thrust output, in Newtons.
        /// </summary>
        public float avgThrustN;
        /// <summary>
        /// The motor's peak thrust output, in Newtons.
        /// </summary>
        public float maxThrustN;
        /// <summary>
        /// The motor's total thrust output, in Newton-seconds.
        /// </summary>
        public float totThrustNs;
        /// <summary>
        /// The length of time that the motor burns.
        /// </summary>
        public float burnTimeS;
        /// <summary>
        /// The number of data files available for this motor, if any.
        /// </summary>
        public int dataFiles;
        /// <summary>
        /// A link to a human-readable detail page on ThrustCurve.org for this engine.
        /// </summary>
        public string infoUrl;
        /// <summary>
        /// The total weight of this motor (casing, fuel, and all) in grams.
        /// </summary>
        /// <remarks>
        /// This only considers the weight of the motor when loaded into the rocket- things like Ground Support
        /// Equipment are not considered.
        /// </remarks>
        public float totalWeightG;
        /// <summary>
        /// The weight of the motor's fuel.
        /// </summary>
        public float propWeightG;
        /// <summary>
        /// A comma-separated list of the delay options for this motor
        /// </summary>
        /// <remarks>
        /// This may be empty, a single element, or multiple elements. The elements might be a single letter, or
        /// an integer number of seconds.
        /// 
        /// There is no real standard for this. Generally, P is understood to mean "plugged" (no charge).
        /// </remarks>
        public string delays;
        /// <summary>
        /// The manufacturer's designation for the casing that this motor is inserted into.
        /// </summary>
        /// <remarks>
        /// This field is only defined for Reloadable motors, and has differing standards based on manufactuer.
        /// For instance, Aerotech uses designations of the form:
        ///   RMS{Diameter}/{Range of lengths supported}
        /// Whereas Cesaroni and partners (AMW, etc) use:
        ///   CTI Pro-{Diameter} {1-6}G{XL?}
        /// Smaller manufacturers use their own formats.
        /// </remarks>
        public string caseInfo;
        /// <summary>
        /// The manufacturer's designation for this motor's propellant formulation
        /// </summary>
        public string propInfo;
        /// <summary>
        /// The last time this entry was updated
        /// </summary>
        /// <remarks>
        /// This happens anytime any data is changed or any new thrust curve files are uploaded.
        /// </remarks>
        public DateTime updatedOn;
        /// <summary>
        /// Whether the motor is still in production.
        /// </summary>
        public string availability;
    }
}