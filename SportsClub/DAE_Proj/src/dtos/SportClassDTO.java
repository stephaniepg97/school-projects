package dtos;

import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

public class SportClassDTO extends ProductDTO {

    private SeasonDTO season;
    private RankDTO rank;
    private TimetableDTO timetable;
    private CoachDTO coach;
    private Set<AttendanceDTO> attendances;

    public SportClassDTO() {
        this.attendances = new LinkedHashSet<>();
    }

    public SportClassDTO(Integer id, String name, double value, int stock, Integer originalId, SeasonDTO season, ModalityDTO modality, RankDTO rank, TimetableDTO timetable, CoachDTO coach) {
        super(id, new CategoryDTO("SportClass"), modality, name, value, stock, originalId);
        this.season = season;
        this.rank = rank;
        this.timetable = timetable;
        this.coach = coach;
        this.attendances = new LinkedHashSet<>();
    }

    public SportClassDTO(String name, double value, int stock, Integer originalId, SeasonDTO season, ModalityDTO modality, RankDTO rank, TimetableDTO timetable, CoachDTO coach) {
        this(null, name, value, stock, originalId, season, modality, rank, timetable, coach);
    }

    public SportClassDTO(String name, double value, int stock, SeasonDTO season, ModalityDTO modality, RankDTO rank, TimetableDTO timetable, CoachDTO coach) {
        this(null, name, value, stock, null, season, modality, rank, timetable, coach);
    }

    public void add(AttendanceDTO r) {
        this.attendances.add(r);
    }

    public void remove(AttendanceDTO r) {
        this.attendances.remove(r);
    }

    public void removeAttendance(int id) {
        List<AttendanceDTO> result = this.attendances.stream().filter(e -> e.getId() == id).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public RankDTO getRank() {
        return rank;
    }

    public void setRank(RankDTO rank) {
        this.rank = rank;
    }

    public TimetableDTO getTimetable() {
        return timetable;
    }

    public void setTimetable(TimetableDTO timetable) {
        this.timetable = timetable;
    }

    public CoachDTO getCoach() {
        return coach;
    }

    public void setCoach(CoachDTO coach) {
        this.coach = coach;
    }

    public SeasonDTO getSeason() {
        return season;
    }

    public void setSeason(SeasonDTO season) {
        this.season = season;
    }

    @Override
    public void reset() {
        super.reset();
        this.season.reset();
        this.coach.reset();
        this.rank.reset();
        this.timetable.reset();
        this.attendances.clear();
    }
}
