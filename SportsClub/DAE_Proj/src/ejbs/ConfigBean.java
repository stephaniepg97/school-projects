package ejbs;

import dtos.*;
import entities.Payment;
import entities.User;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import java.time.DayOfWeek;
import java.time.Instant;
import java.time.LocalTime;
import java.util.Date;
import java.util.logging.Level;
import java.util.logging.Logger;

@Singleton(name = "ConfigBeanEJB")
@Startup
public class ConfigBean {

    @EJB
    private AdministratorBean administratorBean;

    @EJB
    private AssociateBean associateBean;

    @EJB
    private AthleteBean athleteBean;

    @EJB
    private CategoryBean categoryBean;

    @EJB
    private EmailBean emailBean;

    @EJB
    private CoachBean coachBean;

    @EJB
    private SeasonBean seasonBean;

    @EJB
    private GraduationBean graduationBean;

    @EJB
    private ModalityBean modalityBean;

    @EJB
    private PaymentBean paymentBean;

    @EJB
    private GenericProductBean genericProductBean;

    @EJB
    private RankBean rankBean;

    @EJB
    private ReceiptBean receiptBean;

    @EJB
    private RegistrationBean registrationBean;

    @EJB
    private SportClassBean sportClassBean;

    @EJB
    private TimetableBean timetableBean;

    @EJB
    private PurchaseBean purchaseBean;

    private static final Logger logger = Logger.getLogger("ejbs.ConfigBean");

    public ConfigBean() {
    }

    @PostConstruct
    private void populateDB(){
        try {

            administratorBean.create(new AdministratorDTO("admin1", User.hashPassword("123"), "Administrator1", "admin@sudo.com"));

            administratorBean.create(new AdministratorDTO("admin2", User.hashPassword("123"), "Administrator2", "admin2@sudo.com"));

            AssociateDTO associateDTO = associateBean.create(new AssociateDTO("associate", User.hashPassword("123"), "Associate1", "associate@gmail.com"));

            AthleteDTO athleteDTO = athleteBean.create(new AthleteDTO("athlete", User.hashPassword("123"), "Athlete1", "athlete@gmail.com"));

            CategoryDTO categoryDTO = categoryBean.create(new CategoryDTO("Sport Product"));

            ModalityDTO modalityDTO = modalityBean.create(new ModalityDTO("Judo"));

            GraduationDTO graduationDTO = graduationBean.create(new GraduationDTO(modalityDTO, "Yellow belt", 90.0, 10));

            graduationDTO = graduationBean.create(new GraduationDTO(modalityDTO, "Black belt", 100.0, 10));

            //1st: create a purchase (to discover id)
            PurchaseDTO purchaseDTO = purchaseBean.create(new PurchaseDTO(associateDTO, Date.from(Instant.now()).getTime(), 100.0));

            //-----------
            //automatic: define state of payment from -> payed value
            //Product from a payment exclusive to athletes -> graduation
            PaymentDTO paymentDTO = paymentBean.create(new PaymentDTO(purchaseDTO, graduationDTO, Date.from(Instant.now()).getTime(), 1, 50.0, Payment.State.PARTIAL.ordinal()));

            //After all payments are totally (or partially) payed
            ReceiptDTO receiptDTO = receiptBean.create(new ReceiptDTO(purchaseDTO));

            GenericProductDTO genericProductDTO = genericProductBean.create(new GenericProductDTO(graduationDTO.getCategory(), graduationDTO.getName(), 500.0, 12));

            RankDTO rankDTO = rankBean.create(new RankDTO("Junior"));

            TimetableDTO timetableDTO = timetableBean.create(new TimetableDTO(DayOfWeek.MONDAY.getValue(), LocalTime.of(2, 30).toSecondOfDay(), LocalTime.NOON.toSecondOfDay()));

            CoachDTO coachDTO = coachBean.create(new CoachDTO("coach", User.hashPassword("123"), "coach", "coach@gmail.pt"));

            SeasonDTO seasonDTO = seasonBean.create(new SeasonDTO(Date.from(Instant.now()).getTime(), Date.from(Instant.now()).getTime()));

            SportClassDTO sportClassDTO = sportClassBean.create(new SportClassDTO("class1", 100.0, 19, seasonDTO, modalityDTO, rankDTO, timetableDTO, coachDTO));

            RegistrationDTO registrationDTO = registrationBean.create(new RegistrationDTO(athleteDTO));

            //Product from a payment exclusive to associates -> registration
            PaymentDTO paymentDTO1 = paymentBean.create(new PaymentDTO(purchaseDTO, sportClassDTO, Date.from(Instant.now()).getTime(), 1, 50.0, Payment.State.PARTIAL.ordinal()));

            //ProductDTO clone = productBean.clone(productDTO);*/
        }
        catch(Exception e){
            logger.log(Level.SEVERE, e.getMessage());
        }
    }
}